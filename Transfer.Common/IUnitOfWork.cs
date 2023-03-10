using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Transfer.Common
{
    public interface IUnitOfWork : IDisposable
    {
        bool NotChangeLastUpdateTick { get; set; }

        DbContext Context { get; }

        void AutoDetectChangesDisable();

        void AutoDetectChangesEnable();

        void Commit();

        Task CommitAsync(CancellationToken token = default);

        void RollBack();

        Task RollBackAsync(CancellationToken token = default);
        
        void RollBack(bool allChanges);

        Task RollBackAsync(bool allChanges, CancellationToken token = default);

        void BeginTransaction();

        Task BeginTransactionAsync(CancellationToken token = default);

        void SaveChanges();

        void DetachAllEntitys();

        Task SaveChangesAsync(CancellationToken token = default);

        DbSet<T> GetSet<T>() where T : class, IEntityBase;

        Task DeleteAsync<T>(T entity, CancellationToken token = default) where T : class, IEntityBase;

        void Delete<T>(T entity) where T : class, IEntityBase;

        void DeleteList<T>(IEnumerable<T> entities) where T : class, IEntityBase;

        Task DeleteListAsync<T>(IEnumerable<T> entities, CancellationToken token = default) where T : class, IEntityBase;

        T AddEntity<T>(T entity, bool saveChanges = true) where T : class, IEntityBase;

        Task<T> AddEntityAsync<T>(T entity, bool saveChanges = true, CancellationToken token = default) where T : class, IEntityBase;

        T Update<T>(T entity) where T : class, IEntityBase;

        Task<T> UpdateAsync<T>(T entity, CancellationToken token = default) where T : class, IEntityBase;

        TransactionScope GetTransactionScope(IsolationLevel isolationLevel);

        IQueryable<T> Query<T>(bool withDeleted = false) where T : class, IEntityBase;
    }
}
