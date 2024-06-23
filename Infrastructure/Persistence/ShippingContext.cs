﻿using System;
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

            builder.Entity<City>()
                .HasOne(c => c.state)
                    .WithMany(g => g.cities)
                .HasForeignKey(c => c.stateId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Branch>()
                .HasOne(b => b.city)
                .WithOne(c => c.branch)
                .HasForeignKey<Branch>(b => b.cityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.city)
                .WithMany(c => c.cityOrders)
                .HasForeignKey(o => o.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.governorate)
                .WithMany(g => g.governorateOrders)
                .HasForeignKey(o => o.GovernorateId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.merchant)
                .WithMany(m => m.orders)
                .HasForeignKey(o => o.MerchantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.shipping)
                .WithMany(s => s.orders)
                .HasForeignKey(o => o.shippingId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.payment)
                .WithMany(p => p.paymentorders)
                .HasForeignKey(o => o.paymentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Merchant>()
                .HasOne(m => m.city)
                .WithMany(c => c.cityMerchants)
                .HasForeignKey(m => m.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<GovernorateRepresentatives>()
                .HasOne(gr=> gr.governorate)
                .WithMany(g => g.representatives)
                .HasForeignKey(m => m.governorateId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<GovernorateRepresentatives>()
                .HasOne(gr => gr.representative)
                .WithMany(r => r.governorates)
                .HasForeignKey(m => m.representativeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Product>()
                .HasOne(p => p.order)
                .WithMany(o => o.products)
                .HasForeignKey(m => m.orderId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}