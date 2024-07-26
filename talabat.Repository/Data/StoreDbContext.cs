using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using talabat.Repository.Data.Configrations;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;

namespace talabat.Repository.Data
{
     public class StoreDbContext : DbContext
    {

        public StoreDbContext(DbContextOptions<StoreDbContext> options) :base(options) 
        {
            
        }


    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product>  Products { get; set; }


        public DbSet<ProductBrand> Brands { get; set; }


        public DbSet<ProductType> Types { get; set; }


        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }


    }
}
