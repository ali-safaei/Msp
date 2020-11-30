using Infrastructure.Context;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Webframework.Identity
{
  public static  class IdentityConfiguration
    {
        public static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(identityOptions =>
            {
                //Password Settings
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequiredLength = 8;
                identityOptions.Password.RequireNonAlphanumeric = false; //#@!
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireLowercase = true;

                //UserName Settings
                identityOptions.User.RequireUniqueEmail = true;

                //Singin Settings
                //identityOptions.SignIn.RequireConfirmedEmail = false;
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = false;

                //Lockout Settings
                //identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                //identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //identityOptions.Lockout.AllowedForNewUsers = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            //.AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddDefaultTokenProviders();
        }
    }
}
