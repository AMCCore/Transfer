using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common.Extensions;
public static class Repository
{
    public static void AddOrUpdate<T>(this IUnitOfWork uw, ICollection<T> data, Action<T, T> copy) where T : class, IEntityBase
    {
        foreach (var r in data)
        {
            var entity = uw.GetSet<T>().FirstOrDefault(e => e.Id == r.Id);
            if (entity == null)
            {
                uw.AddEntity(r, false);
            }
            else
            {
                copy?.Invoke(r, entity);
            }
        }

        uw.SaveChanges();
    }

    public static void AddOrUpdate<T>(this IUnitOfWork uw, T entity, Action<T, T> copy) where T : class, IEntityBase
    {
        var ext = uw.GetSet<T>().FirstOrDefault(e => e.Id == entity.Id);
        if (ext == null)
        {
            uw.AddEntity(entity, false);
        }
        else
        {
            copy?.Invoke(ext, entity);
        }

        uw.SaveChanges();
    }

    public static async Task AddOrUpdateAsync<T>(this IUnitOfWork uw, ICollection<T> data, Action<T, T> copy, CancellationToken token = default) where T : class, IEntityBase
    {
        foreach (var r in data)
        {
            var entity = await uw.GetSet<T>().FirstOrDefaultAsync(e => e.Id == r.Id, token);
            if (entity == null)
            {
                await uw.AddEntityAsync(r, token: token);
            }
            else
            {
                copy?.Invoke(r, entity);
            }
        }

        await uw.SaveChangesAsync(token);
    }

    public static async Task AddOrUpdateAsync<T>(this IUnitOfWork uw, T entity, Action<T, T> copy, CancellationToken token = default) where T : class, IEntityBase
    {
        var ext = await uw.GetSet<T>().FirstOrDefaultAsync(e => e.Id == entity.Id, token);
        if (ext == null)
        {
            await uw.AddEntityAsync(entity, token: token);
        }
        else
        {
            copy?.Invoke(ext, entity);
        }

        await uw.SaveChangesAsync(token);
    }

    public static void AddIfNotExists<T>(this IUnitOfWork uw, params T[] data) where T : class, IEntityBase
    {
        var entitys = uw.GetSet<T>().Select(x => x.Id).ToArray();
        foreach (var r in data)
        {
            if (entitys.All(x => x != r.Id))
            {
                uw.AddEntity(r, false);
            }
        }
        uw.SaveChanges();
    }

    public static void AddIfNotExists<T>(this IUnitOfWork uw, ICollection<T> data) where T : class, IEntityBase
    {
        var entitys = uw.GetSet<T>().Select(x => x.Id).ToArray();
        foreach (var r in data)
        {
            if (entitys.All(x => x != r.Id))
            {
                uw.AddEntity(r, false);
            }
        }
        uw.SaveChanges();
    }

    /// <summary>
    ///     полуить по
    /// </summary>
    public static T GetById<T>(this IQueryable<T> query, Guid id) where T : class, IEntityBase
    {
        if (id.IsNullOrEmpty()) return null;
        return query.FirstOrDefault(a => a.Id == id);
    }

    /// <summary>
    ///     полуить по
    /// </summary>
    public static async Task<T> GetByIdAsync<T>(this IQueryable<T> query, Guid id, CancellationToken token = default) where T : class, IEntityBase
    {
        if (id.IsNullOrEmpty()) return null;
        return await query.FirstOrDefaultAsync(a => a.Id == id, token);
    }
}
