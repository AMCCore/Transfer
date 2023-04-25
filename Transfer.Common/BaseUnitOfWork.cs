using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;
using Transfer.Common.Extensions;

namespace Transfer.Common;

public class BaseUnitOfWork<C> : IUnitOfWork where C : DbContext
{
    protected C _context;
    protected bool _connectionStringInited;
    protected IDbContextTransaction _transaction;

    /// <summary>
    ///     инициализация UnitOfWork
    /// </summary>
    protected BaseUnitOfWork(C context)
    {
        _connectionStringInited = false;
        _context = context;
    }

    /// <summary>
    ///     без инициализации UnitOfWork
    /// </summary>
    protected BaseUnitOfWork()
    {

    }

    public void BeginTransaction()
    {
        if (_transaction != null)
        {
            throw new TransactionException("The transaction has been already begun");
        }

        _transaction = Context.Database.BeginTransaction();
    }

    public async Task BeginTransactionAsync(CancellationToken token = default)
    {
        if (_transaction != null)
        {
            throw new TransactionException("The transaction has been already begun");
        }
        _transaction = await Context.Database.BeginTransactionAsync(token);
    }

    public bool NotChangeLastUpdateTick { get; set; }

    public DbContext Context => _context;

    public void AutoDetectChangesDisable()
    {
        Context.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public void AutoDetectChangesEnable()
    {
        Context.ChangeTracker.AutoDetectChangesEnabled = true;
    }

    public void Commit()
    {
        SaveChanges();
        _transaction?.Commit();
    }

    public async Task CommitAsync(CancellationToken token = default)
    {
        await SaveChangesAsync(token);
        _transaction?.CommitAsync(token);
    }

    public void DetachAllEntitys()
    {
        foreach (var entry in Context.ChangeTracker.Entries().ToArray())
        {
            entry.State = EntityState.Detached;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        if (_connectionStringInited)
        {
            Context?.Dispose();
        }
    }

    public TransactionScope GetTransactionScope(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = isolationLevel }, TransactionScopeAsyncFlowOption.Enabled);
    }

    public void RollBack()
    {
        _transaction?.Rollback();
    }

    public async Task RollBackAsync(CancellationToken token = default)
    {
        await _transaction?.RollbackAsync(token);
    }

    public void RollBack(bool allChanges)
    {
        RollBack();
        if (allChanges)
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }

    }

    public async Task RollBackAsync(bool allChanges, CancellationToken token = default)
    {
        await RollBackAsync(token);
        if (allChanges)
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }
    }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        ProcessEntityOnSave();
        await Context.SaveChangesAsync(token);
    }

    public void SaveChanges()
    {
        ProcessEntityOnSave();
        Context.SaveChanges();
    }

    public T AddEntity<T>(T entity, bool saveChanges = true) where T : class, IEntityBase
    {
        if (entity.Id.IsNullOrEmpty())
        {
            entity.Id = Guid.NewGuid();
        }

        Context.Set<T>().Add(entity);

        if (saveChanges)
        {
            SaveChanges();
            Context.Entry(entity).State = EntityState.Detached;
            entity = Context.Set<T>().Find(entity.Id);
        }

        return entity;
    }

    async Task<T> IUnitOfWork.AddEntityAsync<T>(T entity, bool saveChanges = true, CancellationToken token = default)
    {
        if (entity.Id.IsNullOrEmpty())
        {
            entity.Id = Guid.NewGuid();
        }

        await Context.Set<T>().AddAsync(entity, token);

        if(saveChanges)
        {
            await SaveChangesAsync(token);
            Context.Entry(entity).State = EntityState.Detached;
            entity = await Context.Set<T>().FindAsync(new object?[] { entity.Id }, cancellationToken: token);
        }

        return entity;
    }

    public void Delete<T>(T entity) where T : class, IEntityBase
    {
        Context.Entry(entity).State = EntityState.Deleted;
        SaveChanges();
    }

    public async Task DeleteAsync<T>(T entity, CancellationToken token) where T : class, IEntityBase
    {
        Context.Entry(entity).State = EntityState.Deleted;
        await SaveChangesAsync(token);
    }

    public void DeleteList<T>(IEnumerable<T> entities) where T : class, IEntityBase
    {
        foreach (var entity in entities)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
        }

        SaveChanges();
    }

    public async Task DeleteListAsync<T>(IEnumerable<T> entities, CancellationToken token)
        where T : class, IEntityBase
    {
        foreach (var entity in entities)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
        }

        await SaveChangesAsync(token);
    }

    public DbSet<T> GetSet<T>() where T : class, IEntityBase
    {
        return Context.Set<T>();
    }

    public async Task<T> UpdateAsync<T>(T entity, CancellationToken token = default) where T : class, IEntityBase
    {
        Context.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync(token);
        Context.Entry(entity).State = EntityState.Detached;
        entity = await Context.Set<T>().FindAsync(new object[] { entity.Id }, token);
        return entity;
    }

    public T Update<T>(T entity) where T : class, IEntityBase
    {
        Context.Entry(entity).State = EntityState.Modified;
        SaveChanges();
        Context.Entry(entity).State = EntityState.Detached;
        entity = Context.Set<T>().Find(entity.Id);
        return entity;
    }

    private void ProcessEntityOnSave()
    {
        var states = new EntityState[] { EntityState.Added, EntityState.Modified };

        var entitystoDelete = Context.ChangeTracker.Entries<ISoftDeleteEntity>()
            .Where(c => c.State == EntityState.Deleted)
            .Select(c => c.Entity)
            .ToList();

        foreach (var entity in entitystoDelete)
        {
            entity.IsDeleted = true;
            Context.Entry(entity).State = EntityState.Modified;
        }

        var entitystoCreate = Context.ChangeTracker.Entries<IEntityWithDateCreated>()
        .Where(c => c.State == EntityState.Added)
        .Select(c => c.Entity)
        .ToList();

        foreach (var entity in entitystoCreate)
        {
            entity.DateCreated = DateTime.Now;
        }


        if (NotChangeLastUpdateTick)
        {
            return;
        }

        var all = Context.ChangeTracker.Entries<IEntityBase>()
            .Select(c => c.Entity)
            .ToList();

        // получение измененных
        var entitys = Context.ChangeTracker.Entries<IEntityBase>()
            .Where(c => states.Contains(c.State))
            .Select(c => c.Entity)
            .ToList();

        // фиксация факта изменений
        foreach (var entity in entitys)
        {
            entity.LastUpdateTick = DateTime.Now.Ticks;
        }


    }

    public IQueryable<T> Query<T>(bool withDeleted = false) where T : class, IEntityBase
    {
        if (withDeleted && typeof(T).GetInterfaces().Contains(typeof(ISoftDeleteEntity)))
        {
            return GetSet<T>().IgnoreQueryFilters();
        }

        return GetSet<T>().AsQueryable();
    }
}
