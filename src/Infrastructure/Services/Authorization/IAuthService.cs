using Domain.Common.Dependencies;
using Infrastructure.Common;
using Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Authorization
{
    public interface IAuthService : IScoped
    {
        Task<JwtToken> LoginAsync(User user);
        Task LogOutAsync();
    }
}
