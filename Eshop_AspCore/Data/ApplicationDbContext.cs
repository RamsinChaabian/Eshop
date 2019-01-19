using Eshop_AspCore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=EshopDB;Trusted_Connection=True;MultipleActiveResultSets=true");

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Products>().HasIndex(c => c.ProductNameFA);
        }


        public DbSet<Category> Tbl_Category { get; set; }
        public DbSet<Comments> Tbl_Comments { get; set; }
        public DbSet<Files> Tbl_Files { get; set; }
        public DbSet<FirstSubCategory> Tbl_FirstSubCategory { get; set; }
        public DbSet<Gallery> Tbl_Gallery { get; set; }
        public DbSet<Products> Tbl_Products { get; set; }
        public DbSet<Role> Tbl_Role { get; set; }
        public DbSet<SecondSubCategory> Tbl_SecondSubCategory { get; set; }
        public DbSet<Server> Tbl_Server { get; set; }
        public DbSet<SettingSite> Tbl_SettingSite { get; set; }
        public DbSet<Slider> Tbl_Slider { get; set; }
        public DbSet<UserAccess> Tbl_UserAccess { get; set; }
        public DbSet<Menu> Tbl_Menu { get; set; }
        public DbSet<LastSubCategory> Tbl_LastSubCategory { get; set; }
        public DbSet<ShoppingCart> Tbl_ShoppingCart { get; set; }
        public DbSet<Weight> Tbl_Weight { get; set; }
        public DbSet<Invoice> Tbl_Invoice { get; set; }

    }
}
