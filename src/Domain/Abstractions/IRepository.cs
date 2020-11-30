using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IRepository<TEntity> : IDisposable
       where TEntity : IBaseEntity
    {
        IUnitOfWork UnitOfWork { get; }
        void Add(TEntity item);
        void Add(IEnumerable<TEntity> items);
        Task AddAsync(TEntity item, CancellationToken cancellationToken);
        Task AddAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken);
        void Remove(TEntity item);
        void Remove(IEnumerable<TEntity> items);
        void Modify(TEntity item);
        void Modify(IEnumerable<TEntity> items);
        void TrackItem(TEntity item);
        void TrackItem(IEnumerable<TEntity> item);
        void Merge(TEntity persisted, TEntity current);
        TEntity Get(object id);
        Task<TEntity> GetAsync(object id,CancellationToken cancellationToken);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);
        Task<IEnumerable<TEntity>> GetPagedAsync<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, CancellationToken cancellationToken);
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken);
        IEnumerable<TEntity> GetFiltered<KProperty>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);
        Task<IEnumerable<TEntity>> GetFilteredAsync<KProperty>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, CancellationToken cancellationToken);
        void Refresh(TEntity entity);

        void SaveChanges();
        Task<int> SaveChangeAsync(CancellationToken cancellationToken);
    }
}
