using Domain.Abstractions;
using Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdateLastLoginAsync(User user, CancellationToken cancellationToken);

    }
}
