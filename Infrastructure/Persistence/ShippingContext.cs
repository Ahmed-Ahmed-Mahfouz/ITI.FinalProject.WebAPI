using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ShippingContext:IdentityDbContext<ApplicationUser,ApplicationRoles,string>
    {
        public ShippingContext(DbContextOptions<ShippingContext> contextOptions) : base(contextOptions)
        { }

        public DbSet<Branch> Branches { get; set; } 
        public DbSet<City> Cities { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Governorate> Governorates { get; set;}
        public DbSet<GovernorateRepresentatives> GovernorateRepresentatives { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<RolePowers> RolePowers { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<SpecialPackages> SpecialPackages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RolePowers>().HasKey("Power", "RoleId");
            builder.Entity<GovernorateRepresentatives>().HasKey("representativeId", "governorateId");
            //builder.Entity<Governorate>().HasData(
            //new Governorate { id = 3, name = "Cairo", status = Status.Active },
            //new Governorate { id = 2, name = "Giza", status = Status.Active }
        //);

            //    builder.Entity<City>().HasData(
            //        new City { id = 1, name = "Nasr City", status = Status.Active, normalShippingCost = 10m, pickupShippingCost = 5m, governorateId = 1 },
            //        new City { id = 2, name = "6th of October", status = Status.Active, normalShippingCost = 15m, pickupShippingCost = 7m, governorateId = 2 }
            //    );

            //    builder.Entity<Branch>().HasData(
            //        new Branch { id = 1, name = "Nasr City Branch", status = Status.Active, addingDate = DateTime.Now, cityId = 1 },
            //        new Branch { id = 2, name = "6th of October Branch", status = Status.Active, addingDate = DateTime.Now, cityId = 2 }
            //    );

            //    builder.Entity<ApplicationUser>().HasData(
            //        new ApplicationUser { Id = "1", FullName = "John Doe", Address = "123 Main St", PhoneNo = "1234567890", Status = Status.Active, BranchId = 1, UserType = UserType.Representative, UserName = "johndoe", NormalizedUserName = "JOHNDOE", Email = "johndoe@example.com", NormalizedEmail = "JOHNDOE@EXAMPLE.COM", EmailConfirmed = true, PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "password"), SecurityStamp = string.Empty, ConcurrencyStamp = string.Empty, PhoneNumber = "1234567890", PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnd = null, LockoutEnabled = true, AccessFailedCount = 0 }
            //    );

            //    builder.Entity<Employee>().HasData(
            //        new Employee { userId = "1" }
            //    );

            //    builder.Entity<Merchant>().HasData(
            //        new Merchant { Id = "2", FullName = "Jane Doe", Address = "456 Main St", PhoneNo = "0987654321", Status = Status.Active, BranchId = 2, UserType = UserType.Merchant, UserName = "janedoe", NormalizedUserName = "JANEDOE", Email = "janedoe@example.com", NormalizedEmail = "JANEDOE@EXAMPLE.COM", EmailConfirmed = true, PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "password"), SecurityStamp = string.Empty, ConcurrencyStamp = string.Empty, PhoneNumber = "0987654321", PhoneNumberConfirmed = true, TwoFactorEnabled = false, LockoutEnd = null, LockoutEnabled = true, AccessFailedCount = 0, StoreName = "Jane's Store", GovernorateId = 1, CityId = 1, MerchantPayingPercentageForRejectedOrders = 10m, SpecialPickupShippingCost = 5m }
            //    );

            //    builder.Entity<Order>().HasData(
            //        new Order { Id = 1, ClientName = "Client 1", Date = DateTime.Now, Phone = "1234567890", VillageAndStreet = "Village 1, Street 1", ShippingToVillage = false, PaymentType = PaymentTypes.CashOnDelivery, Status = OrderStatus.Pending, Type = OrderTypes.HomeDelivery, ShippingCost = 10m, MerchantId = "2", GovernorateId = 1, CityId = 1, ShippingId = 1, BranchId = 1, RepresentativeId = "1" }
            //    );

            builder.Entity<City>()
                .HasOne(c => c.governorate)
                .WithMany(g => g.cities)
                .HasForeignKey(c => c.governorateId)
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
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ShippingId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.branch)
                .WithMany(b => b.branchOrders)
                .HasForeignKey(o => o.BranchId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.representative)
                .WithMany(r => r.representativeOrders)
                .HasForeignKey(o => o.RepresentativeId)
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
                .WithMany(o => o.Products)
                .HasForeignKey(m => m.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SpecialPackages>()
                .HasOne(s => s.governoratePackages)
                .WithMany(g => g.specialPackages)
                .HasForeignKey(s => s.governorateId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SpecialPackages>()
                .HasOne(s => s.merchantSpecialPackage)
                .WithMany(m => m.SpecialPackages)
                .HasForeignKey(s => s.MerchantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SpecialPackages>()
                .HasOne(s => s.cityPackages)
                .WithOne(c => c.citySpecialPackages)
                .HasForeignKey<SpecialPackages>(s => s.cityId)
                .OnDelete(DeleteBehavior.NoAction);


        }

    }
}
