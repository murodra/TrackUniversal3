using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Formula.Api.Core;
using Formula.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Formula.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Lists.All());
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var list = await _unitOfWork.Lists.GetById(id);
            if(list==null) return NotFound();
            return Ok(list);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddList(List list)
        {
            await _unitOfWork.Lists.Add(list);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var list = await _unitOfWork.Lists.GetById(id);
            
            if (list==null) return NotFound();
            
            await _unitOfWork.Lists.Delete(list);
            await _unitOfWork.CompleteAsync();
                    
            return NoContent();
        }

        [HttpPatch]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateList(List list)
        {
            var existList = await _unitOfWork.Lists.GetById(list.Id);
            
            if (existList==null) return NotFound();
            
            await _unitOfWork.Lists.Update(list);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("AddProductToList")]
        public async Task<IActionResult> AddProductToList(Guid listId, Guid productId, int quantity)
        {
            var listProduct = new ListProduct()
            {
                Name ="NN",
                ProductId = productId,
                ListId = listId,
                Quantity = quantity
            };
            await _unitOfWork.ListProducts.Add(listProduct);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPost]
        [Route("RemoveProductFromList")]
        public async Task<IActionResult> RemoveProductFromList(Guid listId, Guid productId)
        {
            var listProduct = _unitOfWork.ListProducts.GetExp(expression: x => x.ListId == listId && x.ProductId == productId , includes: x => x
                             .Include(y => y.List)
                             .Include(y => y.Product));
            await _unitOfWork.ListProducts.Delete(listProduct);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet]
        [Route("FilterByCategory")]
        public async Task<IActionResult> FilterByCategory(Guid categoryId)
        {
            var listProduct =  _unitOfWork.Lists.GetAllExp(expression: x => x.CategoryLists.Any(y=>y.CategoryId==categoryId) , includes: x => x
                             .Include(y => y.ListProducts));
            if(listProduct==null) return NotFound();
            return Ok(listProduct);
        }

        [HttpPost]
        [Route("AddCategoryToList")]
        public async Task<IActionResult> AddCategoryToList(Guid listId, Guid categoryId)
        {
            var categoryList = new CategoryList()
            {
                Name ="NN",
                CategoryId = categoryId,
                ListId = listId,
            };
            await _unitOfWork.CategoryLists.Add(categoryList);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }
    }
}