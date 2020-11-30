using Domain.Common.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable, IScoped
    {

        void Commit();
        Task<int> CommitAsync();
        void Refresh(object entity);
    }
}
