using System.Security.Claims;
using System.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Configurations;
using Formula.Api.Models;
using Formula.Api.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Formula.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthenticationController> _logger;
        // private readonly JwtConfig _jwtConfig;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            ILogger<AuthenticationController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _logger = logger;
            // _jwtConfig = jwtConfig;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);

                if (user_exist!=null)
                {
                    return BadRequest(error:new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>
                        {
                            "Email already exists"
                        }
                    });
                }
                var new_user = new IdentityUser()
                {
                    Email = requestDto.Email,
                    UserName = requestDto.Email
                };

                var is_created = await _userManager.CreateAsync(new_user,requestDto.Password);

                if (is_created.Succeeded)
                {
                    await _userManager.AddToRoleAsync(new_user, "AppUser");

                    var token = await GenerateJwtToken(new_user);

                    return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = token.Token
                    });
                }

                return BadRequest(error:new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Server error"
                    },
                    Result = false
                });
            }

            return BadRequest();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login ([FromBody] UserLoginRequestDto loginRequest)
        {
            if (ModelState.IsValid)
            {
                var existing_user = await _userManager.FindByEmailAsync(loginRequest.Email);
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                if (existing_user == null)
                {
                    return BadRequest(error:new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid payload"
                        },
                        Result=false
                    });
                }

                var isCorrect =await _userManager.CheckPasswordAsync(existing_user, loginRequest.Password);

                if(!isCorrect)
                {
                    return BadRequest(error:new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Invalid Credentials"
                        },
                        Result = false

                    });
                }

                var jwtToken = await GenerateJwtToken(existing_user);

                return Ok(new AuthResult()
                {
                    Token = jwtToken.Token,
                    Result = true
                });
            }

            return BadRequest(error:new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    },
                    Result=false
                });
        }

        // POST: /Account/LogOff
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult LogOff()
        // {
        //     AuthenticationManager.SignOut();
        //     return RedirectToAction("Index", "Home");
        // }

        private async Task<AuthResult> GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection(key:"JwtConfig:Secret").Value);

            var claims = await GetAllValidClaims(user);

            //Token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return new AuthResult(){
                Token =jwtToken,
                Result = true
            };
        }

        //get all valid claims for the user
        private async Task<List<Claim>> GetAllValidClaims(IdentityUser user)
        {
            var _options = new IdentityOptions();

            var claims = new List<Claim>
            {
                new Claim(type:"Id", value:user.Id),
                new Claim(type:JwtRegisteredClaimNames.Sub, value:user.Email),
                new Claim(type:JwtRegisteredClaimNames.Email, value:user.Email),
                new Claim(type:JwtRegisteredClaimNames.Jti, value:Guid.NewGuid().ToString()),
                new Claim(type:JwtRegisteredClaimNames.Iat, value:DateTime.Now.ToUniversalTime().ToString())
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach(var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role!=null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims);
                }
            }

            return claims;
        }
    }
}