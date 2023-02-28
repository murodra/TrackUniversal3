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
    public class ListRepository : GenericRepository<List>, IListRepository
    {
        public ListRepository(ApiDbContext context, ILogger logger,UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, logger, userManager, httpContextAccessor)
        {
        }

        public ApiDbContext Context { get; }
        public ILogger Logger { get; }

        public override async Task<IEnumerable<List>> All()
        {
            try
            {
                return await _context.Lists.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async Task<List?> GetById(Guid id)
        {
            try
            {
                return await _context.Lists.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

    }
}