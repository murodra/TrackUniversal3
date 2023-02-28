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
    public class CategoryProductRepository : GenericRepository<CategoryProduct>, ICategoryProductRepository
    {
        public CategoryProductRepository(ApiDbContext context, ILogger logger, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor) : base(context, logger, userManager, httpContextAccessor)
        {
        }

    }
}