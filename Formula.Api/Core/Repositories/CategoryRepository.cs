using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackUniversal2.Entities;

namespace Formula.Api.Core.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public ApiDbContext Context { get; }
        public ILogger Logger { get; }

        public CategoryRepository(ApiDbContext context, ILogger logger,UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, logger, userManager, httpContextAccessor)
        {
        }


        public override async Task<IEnumerable<Category>> All()
        {
            try
            {
                return await _context.Categorys.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async Task<Category?> GetById(Guid id)
        {
            try
            {
                return await _context.Categorys.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            }
            catch (System.Exception)
            {

                throw;
            }
        }



    }
}