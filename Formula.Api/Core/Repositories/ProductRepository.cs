using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Formula.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Formula.Api.Core.Repositories
{   
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private ApiDbContext context;
        private ILogger logger;

        public ProductRepository(ApiDbContext context, ILogger logger,UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, logger, userManager, httpContextAccessor)
        {
        }

        public override async Task<IEnumerable<Product>> All()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async Task<Product?> GetById(Guid id)
        {
            try
            {
                return await _context.Products.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // public async Task<Product?> GetByProductNo(int listNo)
        // {
        //     try
        //     {
        //         return await _context.Products.FirstOrDefaultAsync(x=>x.ProductNumber == listNo);
        //     }
        //     catch (System.Exception)
        //     {
                
        //         throw;
        //     }
        // }

    }
    
}