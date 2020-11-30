using Domain.Common.Dependencies;
using Infrastructure.Common;
using Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Settings
{
    public interface IJwtService : IScoped
    {
        Task<JwtToken> CreateAsync(User user);
    }
}
