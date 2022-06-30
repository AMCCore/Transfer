using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Enums;
using Transfer.Dal.Entities;
using Transfer.Dal.Extensions;
using Transfer.Dal.Helpers;

namespace Transfer.Dal.Context
{
    /// <summary>
    ///     контекст работы с БД
    /// </summary>
    public partial class TransferContext : DbContext
    {
        public TransferContext(DbContextOptions<TransferContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<DbAccountRight>().HasIndex(a => new { a.AccountId, a.RightId, a.OrganisationId }).IsUnique();
            modelBuilder.Entity<DbExternalLogin>().Property(d => d.LoginType).HasConversion(new GuidEnumConverter<ExternalLoginEnum>());
            modelBuilder.Entity<DbExternalLogin>().HasIndex(d => new { d.LoginType, d.Value }).IsUnique();
            modelBuilder.Entity<DbOrganisationAccount>().Property(d => d.AccountType).HasConversion(new GuidEnumConverter<OrganisationAccountType>());
            modelBuilder.Entity<DbOrganisationFile>().Property(d => d.FileType).HasConversion(new GuidEnumConverter<OrganisationFileType>());
            modelBuilder.Entity<DbDriverFile>().Property(d => d.FileType).HasConversion(new GuidEnumConverter<DriverFileType>());
            modelBuilder.Entity<DbBusFile>().Property(d => d.FileType).HasConversion(new GuidEnumConverter<BusFileType>());
            modelBuilder.Entity<DbBus>().Property(d => d.State).HasConversion(new GuidEnumConverter<BusStateEnum>());
            modelBuilder.Entity<DbDriver>().Property(d => d.State).HasConversion(new GuidEnumConverter<DriverStateEnum>());
            modelBuilder.Entity<DbOrganisation>().Property(d => d.State).HasConversion(new GuidEnumConverter<OrganisationStateEnum>());
            modelBuilder.Entity<DbTripRequest>().Property(d => d.State).HasConversion(new GuidEnumConverter<TripRequestStateEnum>());

            //modelBuilder.Entity<DbTripRequestIdentifier>().Property(a => a.Identifier).ValueGeneratedOnAdd();
            modelBuilder.Entity<DbTripRequestIdentifier>().HasIndex(a => new { a.TripRequestId }).IsUnique();
            modelBuilder.Entity<DbTripRequestIdentifier>().HasIndex(a => new { a.Identifier }).IsUnique();

            modelBuilder.DisableCascadeDeleteConvention();

            modelBuilder.UseIdentityColumns();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Select(x => x.ClrType).Where(x => typeof(ISoftDeleteEntity).IsAssignableFrom(x)))
            {
                var parameter = Expression.Parameter(entityType, "e");
                var body = Expression.Equal(
                    Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter,
                    Expression.Constant("IsDeleted")),
                    Expression.Constant(false));
                modelBuilder.Entity(entityType).HasQueryFilter(Expression.Lambda(body, parameter));
            }

            //modelBuilder.SetUnderscoreSnakeConventions();
            
        }
    }
}
