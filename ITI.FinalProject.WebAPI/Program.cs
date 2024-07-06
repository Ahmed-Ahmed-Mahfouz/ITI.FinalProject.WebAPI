using Application;
using Domain;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ITI.FinalProject.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string txt = "";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
            //builder.Services.AddScoped<IRepresentativeRepo, RepresentativeRepo>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(txt,
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SKey").Value??""))
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{

            app.UseSwagger();
            app.UseSwaggerUI();

            //}

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(txt);

            app.MapControllers();

            await EnsureAdminRoleExistsAsync(app);

            await EnsureMerchantRoleExistsAsync(app);
            
            await EnsureRepresentativeRoleExistsAsync(app);

            await EnsureAdminExistsAsync(app);

            await app.RunAsync();
        }

        private static async Task EnsureAdminExistsAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var user = await userManager.FindByNameAsync("admin");
                if (user == null)
                {
                    user = new ApplicationUser 
                    { 
                        Id = Guid.NewGuid().ToString(),
                        UserName = "admin",
                        Email = "admin@example.com",
                        Status  = Domain.Enums.Status.Active,
                        UserType = Domain.Enums.UserType.Admin,
                        FullName = "admin",
                        Address = "Cairo"
                    };

                    var result = await userManager.CreateAsync(user, "Password123!");
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                    else
                    {
                        result = await userManager.AddToRoleAsync(user, "Admin");

                        if (!result.Succeeded)
                        {
                            throw new Exception($"Failed to assign user to role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while creating the user.");
            }
        }

        private static async Task EnsureAdminRoleExistsAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRoles>>();
                var role = await roleManager.FindByNameAsync("Admin");
                if (role == null)
                {
                    role = new ApplicationRoles { Id = Guid.NewGuid().ToString(), Name = "Admin",TimeOfAddition = DateTime.Now, NormalizedName = "ADMIN" };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while creating the role.");
            }
        }

        private static async Task EnsureMerchantRoleExistsAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRoles>>();
                var role = await roleManager.FindByNameAsync("Merchant");
                if (role == null)
                {
                    role = new ApplicationRoles { Id = Guid.NewGuid().ToString(), Name = "Merchant", TimeOfAddition = DateTime.Now, NormalizedName = "MERCHANT" };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while creating the role.");
            }
        }

        private static async Task EnsureRepresentativeRoleExistsAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRoles>>();
                var role = await roleManager.FindByNameAsync("Representative");
                if (role == null)
                {
                    role = new ApplicationRoles { Id = Guid.NewGuid().ToString(), Name = "Representative", TimeOfAddition = DateTime.Now, NormalizedName = "REPRESENTATIVE" };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while creating the role.");
            }
        }
    }
}
