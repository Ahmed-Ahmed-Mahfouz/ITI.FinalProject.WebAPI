using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ShippingContext:IdentityDbContext<ApplicationUser,ApplicationRoles,string>
    {
        public DbSet<Branch> Branches { get; set; } 
        public DbSet<City>Cities { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Governorate> Governorates { get; set;}
        public DbSet<GovernorateRepresentatives> GovernorateRepresentatives { get; set; }
        public DbSet<Merchant>Merchants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment>Payments { get; set; }
        public DbSet<Product>Products { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<RolePowers> RolePowers { get; set; }
        public DbSet<Shipping>Shippings { get; set; }

        public ShippingContext(DbContextOptions<ShippingContext> contextOptions):base(contextOptions) 
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RolePowers>().HasKey("Power", "RoleId");
            builder.Entity<GovernorateRepresentatives>().HasKey("representativeId", "governorateId");
        }

    }
}
