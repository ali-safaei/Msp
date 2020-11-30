using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.DataInitializer
{
    public class UserInitializer : IDataInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UserInitializer(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Init()
        {
            #region add admin role
            var adminRole = _roleManager.Roles.AsNoTracking().SingleOrDefault(a => a.Name == "admin");
            if (adminRole == null)
            {
                var role = new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "admin",
                    NormalizedName = "ADMIN"
                };
                _roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }
            #endregion
            #region add admin

            if (!_userManager.Users.AsNoTracking().Any(p => p.UserName == "admin"))
            {

                var user = new User
                {
                    FirstName = "Super",
                    LastName = "Administrator",
                    UserName = "admin",
                    Email = "admin@nmv.com",
                    LastModifiedDate = DateTime.UtcNow,
                    IsSuperAdministrator = true,
                    IsActive = true
                };

                var result = _userManager.CreateAsync(user, "R00tadmin@").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var adminUser = _userManager.Users.FirstOrDefaultAsync(a => a.UserName.ToLower() == "admin").GetAwaiter().GetResult();
                    var result_ = _userManager.AddToRoleAsync(adminUser, "admin").GetAwaiter().GetResult();
                }

            }
            #endregion
        }

    }
}
