using Common.Utilities;
using Domain.Abstractions;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        private readonly IInfraUnitOfWork _unitOfWork;
        public Repository(IInfraUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public void Add(TEntity item)
        {
            Assert.NotNull(item, nameof(TEntity));
            Entities.Add(item);
        }

        public void Add(IEnumerable<TEntity> items)
        {
            Assert.NotNull(items, nameof(TEntity));
            Entities.AddRange(items);
        }

        public async Task AddAsync(TEntity item, CancellationToken cancellationToken)
        {
            Assert.NotNull(item, nameof(TEntity));
            await Entities.AddAsync(item, cancellationToken);
        }

        public async Task AddAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken)
        {
            Assert.NotNull(items, nameof(TEntity));
            await Entities.AddRangeAsync(items, cancellationToken);
        }

        public void Dispose()
        {
            if (UnitOfWork != null)
                UnitOfWork.Dispose();
        }

        public TEntity Get(object id)
        {
            return Entities.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Entities;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Entities.ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetAsync(object id, CancellationToken cancellationToken)
        {
            return await Entities.FindAsync(id,cancellationToken);
        }

        public IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            return Entities.Where(filter);
        }

        public IEnumerable<TEntity> GetFiltered<KProperty>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending)
        {
            var q = Entities.Where(filter);
            if (ascending) q.OrderBy(orderByExpression); else q.OrderByDescending(orderByExpression);
            return q.Skip(pageCount * pageIndex).Take(pageCount);
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken)
        {
            return await Entities.Where(filter).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetFilteredAsync<KProperty>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, CancellationToken cancellationToken)
        {
            var q = Entities.Where(filter);
            if (ascending) q.OrderBy(orderByExpression); else q.OrderByDescending(orderByExpression);
            return await q.Skip(pageCount * pageIndex).Take(pageCount).ToListAsync(cancellationToken);
        }

        public IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending)
        {
            var q = Entities.AsQueryable();
            if (ascending) q.OrderBy(orderByExpression); else q.OrderByDescending(orderByExpression);
            return q.Skip(pageCount * pageIndex).Take(pageCount);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, CancellationToken cancellationToken)
        {
            var q = Entities.AsQueryable();
            if (ascending) q.OrderBy(orderByExpression); else q.OrderByDescending(orderByExpression);
            return await q.Skip(pageCount * pageIndex).Take(pageCount).ToListAsync(cancellationToken);
        }

        public void Merge(TEntity persisted, TEntity current)
        {
            _unitOfWork.ApplyCurrentValues(persisted, current);
        }

        public void Modify(TEntity item)
        {
            Assert.NotNull(item, nameof(TEntity));
            _unitOfWork.SetModified(item);
        }

        public void Modify(IEnumerable<TEntity> items)
        {
            Assert.NotNull(items, nameof(TEntity));
            foreach (var item in items)
                _unitOfWork.SetModified(item);
        }



        public void Refresh(TEntity entity)
        {
            _unitOfWork.Refresh(entity);
        }

        public void Remove(TEntity item)
        {
            Assert.NotNull(item, nameof(TEntity));
            Entities.Remove(item);
        }

        public void Remove(IEnumerable<TEntity> items)
        {
            Assert.NotNull(items, nameof(TEntity));
            Entities.RemoveRange(items);
        }

        public void TrackItem(TEntity item)
        {
            Assert.NotNull(item, nameof(TEntity));
            _unitOfWork.Attach<TEntity>(item);
        }

        public void TrackItem(IEnumerable<TEntity> items)
        {
            Assert.NotNull(items, nameof(TEntity));
            foreach (var item in items)
                _unitOfWork.Attach<TEntity>(item);
        }

        public DbSet<TEntity> Entities { get => SetEntity(); }

        DbSet<TEntity> SetEntity()
        {
            return _unitOfWork.Set<TEntity>();
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.CommitAsync();
        }
    }
}
