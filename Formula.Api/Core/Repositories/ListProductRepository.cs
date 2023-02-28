using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data;
using Formula.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Formula.Api.Core.Repositories
{
    public class ListProductRepository : GenericRepository<ListProduct>, IListProductRepository
    {
        private ApiDbContext context;
        private ILogger logger;


        public ListProductRepository(ApiDbContext context, ILogger logger,UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, logger, userManager, httpContextAccessor)
        {
        }

        public override async Task<IEnumerable<ListProduct>> All()
        {
            try
            {
                return await _context.ListProducts.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async Task<ListProduct?> GetById(Guid id)
        {
            try
            {
                return await _context.ListProducts.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }


        // public async Task<ListProduct?> GetByListProductNo(int listNo)
        // {
        //     try
        //     {
        //         return await _context.ListProducts.FirstOrDefaultAsync(x=>x.ListProductNumber == listNo);
        //     }
        //     catch (System.Exception)
        //     {
                
        //         throw;
        //     }
        // }

    }
}