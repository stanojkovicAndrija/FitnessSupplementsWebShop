using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessSupplementsWebShop.Entities
{
    public class FitnessSupplementsWebShopContext : DbContext
    {
        public FitnessSupplementsWebShopContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UsersEntity> Users { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<ManufacturerEntity> Manufacturer { get; set; }
        public DbSet<OrderitemEntity> Orderitem { get; set; }
        public DbSet<OrdersEntity> Orders { get; set; }
        public DbSet<PaymentEntity> Payment { get; set; }
        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<ReviewEntity> Review { get; set; }
        
    }
}
