using Common.Resources;
using Infrastructure.Identity.Models;
using Infrastructure.Models.Auth;
using Infrastructure.Repositories.Users;
using Infrastructure.Services.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msp.Api.Controllers
{
    public class AccountController : ApiBaseController
    {
        #region Fields
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        #endregion
        #region CTOR
        public AccountController(IUserRepository userRepository, IAuthService authService, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _authService = authService;
            _userManager = userManager;
        }
        #endregion
        #region register new user      

        #endregion
        #region login
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                throw new Exception(AccountResources.username_password_notvalid);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
                throw new Exception(AccountResources.username_password_notvalid);
            if (!user.IsActive)
                throw new Exception(AccountResources.user_notactive);
            var jwt = await _authService.LoginAsync(user);
            return Ok(jwt.Token);
        }
        #endregion
        #region log out
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _authService.LogOutAsync();
            return Ok();
        }

        #endregion
    }
}
