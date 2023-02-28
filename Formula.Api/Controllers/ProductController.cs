using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Formula.Api.Core;
using Formula.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Formula.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Products.All());
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _unitOfWork.Products.GetById(id);
            if(product==null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            await _unitOfWork.Products.Add(product);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _unitOfWork.Products.GetById(id);
            
            if (product==null) return NotFound();
            
            await _unitOfWork.Products.Delete(product);
            await _unitOfWork.CompleteAsync();
                    
            return NoContent();
        }

        [HttpPatch]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var existProduct = await _unitOfWork.Products.GetById(product.Id);
            
            if (existProduct==null) return NotFound();
            
            await _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        
    }
}