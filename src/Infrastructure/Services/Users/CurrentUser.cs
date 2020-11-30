using Application.Abstractions.Identity;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Users
{
    public class CurrentUser : ICurrentUser
    {

        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => GetUserId();

        public bool IsAuthenticated => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        private string GetUserId()
        {
            if (IsAuthenticated)
                return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return string.Empty;
        }

    }
}
