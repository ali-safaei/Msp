using Infrastructure.Common;
using Infrastructure.Identity.Models;
using Infrastructure.Services.Settings;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Authorization
{
    public class AuthService : IAuthService
    {
        #region Fields
        private readonly IJwtService _jwtService;
        private readonly SignInManager<User> _signInManager;
        #endregion
        #region CTOR
        public AuthService(IJwtService jwtService, SignInManager<User> signInManager)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
        }
        #endregion

        public async Task<JwtToken> LoginAsync(User user)
        {
            var token = await _jwtService.CreateAsync(user);
            await _signInManager.SignInAsync(user, false);
            return token;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
