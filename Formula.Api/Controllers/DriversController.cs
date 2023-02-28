// using System.Runtime.InteropServices.WindowsRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data;
using Formula.Api.Core;
using Formula.Api.Data;
using Formula.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Formula.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //, Roles ="Admin"
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public DriversController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Drivers.All());
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var driver = await _unitOfWork.Drivers.GetById(id);
            if(driver==null) return NotFound();
            return Ok(driver);
        }

        [HttpPost]
        [Route("AddDriver")]
        public async Task<IActionResult> AddDriver(Driver driver)
        {
            await _unitOfWork.Drivers.Add(driver);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var driver = await _unitOfWork.Drivers.GetById(id);

            if (driver==null) return NotFound();

            await _unitOfWork.Drivers.Delete(driver);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpPatch]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateDriver(Driver driver)
        {
            var existDriver = await _unitOfWork.Drivers.GetById(driver.Id);

            if (existDriver==null) return NotFound();

            await _unitOfWork.Drivers.Update(driver);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}