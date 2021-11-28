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

        void RollBack();

        void RollBack(bool allChanges);

        void BeginTransaction();

        void SaveChanges();

        void DetachAllEntitys();

        Task SaveChangesAsync(CancellationToken token = default);

        DbSet<T> GetSet<T>() where T : class, IEntityBase;

        Task DeleteAsync<T>(T entity, CancellationToken token = default) where T : class, IEntityBase;

        void Delete<T>(T entity) where T : class, IEntityBase;

        void DeleteList<T>(IEnumerable<T> entities) where T : class, IEntityBase;

        Task DeleteListAsync<T>(IEnumerable<T> entities, CancellationToken token = default) where T : class, IEntityBase;

        T AddEntity<T>(T entity, bool saveChanges = true) where T : class, IEntityBase;

        Task<T> AddEntityAsync<T>(T entity, CancellationToken token = default) where T : class, IEntityBase;

        Task<T> AddEntityWithoutSaveAsync<T>(T entity, CancellationToken token = default) where T : class, IEntityBase;

        T Update<T>(T entity) where T : class, IEntityBase;

        Task<T> UpdateAsync<T>(T entity, CancellationToken token = default) where T : class, IEntityBase;

        TransactionScope GetTransactionScope(IsolationLevel isolationLevel);

        IQueryable<T> Query<T>(bool withDeleted = false) where T : class, IEntityBase;
    }
}
