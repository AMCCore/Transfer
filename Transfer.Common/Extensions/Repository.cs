using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common.Extensions
{
    public static class Repository
    {
        public static void AddOrUpdate<T>(this IUnitOfWork uw, ICollection<T> data, Action<T, T> copy) where T : class, IEntityBase
        {
            var entitys = uw.GetSet<T>().ToArray();

            foreach (var r in data)
            {
                var entity = entitys.FirstOrDefault(e => e.Id == r.Id);
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
}
