using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Formula.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Formula.Api.Core.Repositories
{
    public class DriverRepository : GenericRepository<Driver>, IDriverRepository
    {
        public ApiDbContext Context { get; }
        public ILogger Logger { get; }

        public DriverRepository(ApiDbContext context, ILogger logger,UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, logger, userManager, httpContextAccessor)
        {
        }


        public override async Task<IEnumerable<Driver>> All()
        {
            try
            {
                return await _context.Drivers.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async Task<Driver?> GetById(Guid id)
        {
            try
            {
                return await _context.Drivers.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<Driver?> GetByDriverNo(int driverNo)
        {
            try
            {
                return await _context.Drivers.FirstOrDefaultAsync(x=>x.DriverNumber == driverNo);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

    }
}