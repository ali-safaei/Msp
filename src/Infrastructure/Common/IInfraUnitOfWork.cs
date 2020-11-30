using Domain.Abstractions;
using Domain.Common;
using Domain.Common.Dependencies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Common
{
    public interface IInfraUnitOfWork : IUnitOfWork, IScoped
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        void Attach<TEntity>(TEntity item) where TEntity : class;
        void SetModified<TEntity>(TEntity item) where TEntity : class;
        void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class;

    }
}
