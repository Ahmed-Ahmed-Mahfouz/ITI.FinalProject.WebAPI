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
        public static void Main(string[] args)
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
                    builder.AllowAnyHeader().
                            AllowAnyMethod().
                            AllowAnyOrigin();
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SKey").Value??""))
                };
            }
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{

            app.UseSwagger();
            app.UseSwaggerUI();

            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(txt);

            app.MapControllers();

            app.Run();
        }
    }
}
