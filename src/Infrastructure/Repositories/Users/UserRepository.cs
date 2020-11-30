using Domain.Abstractions;
using Domain.Common.Dependencies;
using Infrastructure.Common;
using Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Users
{
    public class UserRepository : Repository<User>, IUserRepository, IScoped
    {
        public UserRepository(IInfraUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public async Task UpdateLastLoginAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDateUtc = DateTime.UtcNow;
            Modify(user);
            await SaveChangeAsync(cancellationToken);
        }
    }
}
