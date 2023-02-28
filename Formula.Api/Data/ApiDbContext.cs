using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Formula.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackUniversal2.Entities;

namespace Data
{
    public class ApiDbContext:IdentityDbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<ListProduct> ListProducts { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<CategoryList> CategoryLists { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ListProduct>()
                .HasKey(cs => new { cs.ListId, cs.ProductId });
            modelBuilder.Entity<CategoryProduct>()
                .HasKey(cs => new { cs.CategoryId, cs.ProductId });
            modelBuilder.Entity<CategoryList>()
                .HasKey(cs => new { cs.CategoryId, cs.ListId  });
        }
        public ApiDbContext(DbContextOptions<ApiDbContext> options):base(options)
        {
            
        }
    }
}