using Infrastructure.Context;
using Infrastructure.DataInitializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Webframework.Database
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddCustomizedDb(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                if (!environment.IsProduction())
                {
                    opt.EnableDetailedErrors();
                }
            });
            return services;
        }


        public static void IntializeDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>(); //Service locator

            //Dos not use Migrations, just Create Database with latest changes
            //dbContext.Database.EnsureCreated();
            //Applies any pending migrations for the context to the database like (Update-Database)
            dbContext.Database.Migrate();

            var dataInitializers = scope.ServiceProvider.GetServices<IDataInitializer>();
            var cd = dataInitializers.Count();
            foreach (var dataInitializer in dataInitializers)
                dataInitializer.Init();
        }
    }


   

}
