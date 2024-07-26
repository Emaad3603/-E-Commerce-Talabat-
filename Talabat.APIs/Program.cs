using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using talabat.Repository.Data;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterFaces;
using Talabat.Repository.Repositories;
using Talabat.APIs.Extensions;
using StackExchange.Redis;
using Talabat.Repository.Identity;
using Talabat.Repository.Identity.DataSeed;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.InterFaces;
using Talabat.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
namespace Talabat.APIs
{
    public class Program
    {
        //Entry Point 
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region ConfigureServices


            webApplicationBuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();
            webApplicationBuilder.Services.AddDbContext<StoreDbContext>(option =>
            {
                option.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            webApplicationBuilder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });

            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider)=>
            {
                var configurations = webApplicationBuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(configurations);
            });
         //   webApplicationBuilder.Services.AddScoped<IBasketRepository, BasketRepository>();
            webApplicationBuilder.Services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));
            webApplicationBuilder.Services.addApplicationServices();
            webApplicationBuilder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
               //allow services
            }
            ).AddEntityFrameworkStores<AppIdentityDbContext>();

            webApplicationBuilder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                                          .AddJwtBearer(options =>
                                          {
                                              options.TokenValidationParameters = new TokenValidationParameters()
                                              {
                                                  ValidateIssuer = true,
                                                  ValidIssuer = webApplicationBuilder.Configuration["JWT:ValidIssuer"],
                                                  ValidateAudience = true,
                                                  ValidAudience = webApplicationBuilder.Configuration["JWT:ValidAudience"],
                                                  ValidateLifetime = true,
                                                  ValidateIssuerSigningKey = true,
                                                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationBuilder.Configuration["JWT:Key"]))

                                              };
                                          });


            webApplicationBuilder.Services.AddScoped<ITokenServices, TokenServices>();
            webApplicationBuilder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", config =>
                {
                    config.AllowAnyHeader();
                    config.AllowAnyMethod();
                    config.WithOrigins(webApplicationBuilder.Configuration["FrontEndBaseUrl"]);
                });
            });
            #endregion

             #region Services 

            var app = webApplicationBuilder.Build();


            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _context = services.GetRequiredService<StoreDbContext>();


            var _identityDbContext = services.GetRequiredService<AppIdentityDbContext>();



            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
               await  _context.Database.MigrateAsync();
               await  StoreDbContextSeed.SeedAsync(_context);

               await _identityDbContext.Database.MigrateAsync();


               var _userManager =services.GetRequiredService<UserManager<AppUser>>();
               await AppIdentityDbContextSeed.SeedUserAsync(_userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an Error has been accured during applying the migration");

            }
            #endregion



            #region Configure
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            
            #endregion
            app.Run();
        }
    }
}
