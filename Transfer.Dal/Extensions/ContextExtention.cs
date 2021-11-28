using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Dal.Extensions
{
    internal static class ContextExtention
    {
        internal static void DisableCascadeDeleteConvention(this ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        internal static void SetMaxLengthOfTextColumn(this ModelBuilder modelBuilder)
        {
            //todo создать универсальный механизм без 
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    var columnType = property.GetColumnType();

                    //property.SetColumnType
                    //property.SetColumnName(SnakeCase(Cleanup(property.Name)));
                }
            }
        }
    }
}
