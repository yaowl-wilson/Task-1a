using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace CartService.Contexts
{
    public class CartModelContext : DbContext
    {
        public CartModelContext() { }
        public CartModelContext(DbContextOptions<CartModelContext> options): base(options) { }
        public DbSet<CartModel> CartModelItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<CartModel>().HasNoKey();
            //modelBuilder.Entity<ProductModel>().HasNoKey();
        }
    }
}
