using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using TrackUniversal2.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Formula.Api.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected ApiDbContext _context;
        internal DbSet<T> _dbSet;
        protected readonly ILogger _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GenericRepository(
            ApiDbContext context,
            ILogger logger,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _context = context;
            _logger = logger;
            this._dbSet = context.Set<T>();
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);

            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTimeOffset.UtcNow;
            entity.UpdatedDate = null;
            entity.IsActive = true;
                      
            var mail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(mail);
            entity.CreatedById = Guid.Parse(user.Id);

            return true;
        }

        public virtual async Task<bool> Delete(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }

        public virtual async Task<T?> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Update(T entity)
        {
            _dbSet.Update(entity);
            
            var mail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(mail);
            entity.UpdatedDate =DateTimeOffset.UtcNow;
            entity.UpdatedById = Guid.Parse(user.Id);
            
            return true;
        }

        public T GetExp(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            var query = _dbSet.AsQueryable();
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                query = includes(query);
            }
            return query.FirstOrDefault(x=>x.IsActive);
        }

        public List<T> GetAllExp(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            var query = _dbSet.AsQueryable();
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                query = includes(query);
            }
            return query.Where(x => x.IsActive).ToList();
        }

    }
}