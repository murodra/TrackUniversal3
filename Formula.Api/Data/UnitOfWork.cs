using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Formula.Api.Core;
using Formula.Api.Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Formula.Api.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApiDbContext _context;
        private readonly ILogger _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UnitOfWork(
            IDriverRepository drivers,
            IListRepository lists,
            IProductRepository products,
            ICategoryRepository categorys,
            ICategoryListRepository categoryLists,
            ICategoryProductRepository categoryProducts,
            IListProductRepository listProducts)
        {
            this.Drivers = drivers;
            this.Products = products;
            this.Lists = lists;
            this.ListProducts = listProducts;
            this.Categorys = categorys;
            this.CategoryLists = categoryLists;
            this.CategoryProducts = categoryProducts;
        }

        public IDriverRepository Drivers { get; private set;}
        public IListRepository Lists { get; private set;}
        public IProductRepository Products { get; private set;}
        public IListProductRepository ListProducts { get; private set;}
        public ICategoryRepository Categorys { get; private set;}
        public ICategoryProductRepository CategoryProducts { get; private set;}
        public ICategoryListRepository CategoryLists { get; private set;}

        

        public UnitOfWork(
            ApiDbContext context,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            var _logger = loggerFactory.CreateLogger(categoryName:"logs");
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

            Drivers = new DriverRepository(_context, _logger, _userManager, _httpContextAccessor);
            Lists = new ListRepository(_context, _logger, _userManager, _httpContextAccessor);
            Products = new ProductRepository(_context, _logger, _userManager, _httpContextAccessor);
            ListProducts = new ListProductRepository(_context, _logger, _userManager, _httpContextAccessor);
            Categorys = new CategoryRepository(_context, _logger, _userManager, _httpContextAccessor);
            CategoryLists = new CategoryListRepository(_context, _logger, _userManager, _httpContextAccessor);
            CategoryProducts = new CategoryProductRepository(_context, _logger, _userManager, _httpContextAccessor);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}