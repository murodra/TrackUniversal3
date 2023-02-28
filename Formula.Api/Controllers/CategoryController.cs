using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Formula.Api.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrackUniversal2.Entities;

namespace Formula.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="admin")] //
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CategoryController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,IHttpContextAccessor httpContextAccessor)
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
            return Ok(await _unitOfWork.Categorys.All());
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var category = await _unitOfWork.Categorys.GetById(id);
            if(category==null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory(Category category)
        {
            await _unitOfWork.Categorys.Add(category);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _unitOfWork.Categorys.GetById(id);

            if (category==null) return NotFound();

            await _unitOfWork.Categorys.Delete(category);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpPatch]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var existCategory = await _unitOfWork.Categorys.GetById(category.Id);

            if (existCategory==null) return NotFound();

            await _unitOfWork.Categorys.Update(category);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}