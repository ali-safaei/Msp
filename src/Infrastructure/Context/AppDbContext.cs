using Common.Utilities;
using Domain.Abstractions;
using Domain.Common;
using Infrastructure.Common;
using Infrastructure.Common.Utilities;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<User, Role, string>, IInfraUnitOfWork
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var entitiesAssembly = typeof(IBaseEntity).Assembly;
            var infraLayerAssembly = typeof(InfrastructureLayer).Assembly;
            modelBuilder.RegisterAllEntities<IBaseEntity>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(infraLayerAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
            modelBuilder.AddPluralizingTableNameConvention();
        }
        #region overrides
        public override int SaveChanges()
        {
            _cleanString();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.Fa2En().FixPersianChars();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }
        #endregion
        #region unit of work members
        public void Commit()
        {
            this.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await this.SaveChangesAsync();
        }

        public virtual void Attach<TEntity>(TEntity item)
            where TEntity : class
        {
            base.Entry<TEntity>(item).State = EntityState.Unchanged;
        }
        public virtual void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            base.Entry<TEntity>(item).State = EntityState.Modified;
        }
        public virtual void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            base.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }
        public virtual void Refresh(object entity)
        {
            base.Entry(entity).Reload();
        }
        #endregion
    }
}