﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Transfer.Common;
using Transfer.Common.Enums;
using Transfer.Common.Enums.States;
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
            
            modelBuilder.Entity<DbExternalLogin>().Property(d => d.LoginType).HasConversion(new GuidEnumConverter<ExternalLoginTypeEnum>());
            //modelBuilder.Entity<DbExternalLogin>().HasAlternateKey(d => new { d.LoginType, d.Value });
            modelBuilder.Entity<DbOrganisationAccount>().Property(d => d.AccountType).HasConversion(new GuidEnumConverter<OrganisationAccountTypeEnum>());
            modelBuilder.Entity<DbOrganisationFile>().Property(d => d.FileType).HasConversion(new GuidEnumConverter<OrganisationFileTypeEnum>());
            modelBuilder.Entity<DbDriverFile>().Property(d => d.FileType).HasConversion(new GuidEnumConverter<DriverFileTypeEnum>());
            modelBuilder.Entity<DbBusFile>().Property(d => d.FileType).HasConversion(new GuidEnumConverter<BusFileTypeEnum>());
            modelBuilder.Entity<DbBus>().Property(d => d.State).HasConversion(new GuidEnumConverter<BusStateEnum>());
            modelBuilder.Entity<DbDriver>().Property(d => d.State).HasConversion(new GuidEnumConverter<DriverStateEnum>());
            modelBuilder.Entity<DbOrganisation>().Property(d => d.State).HasConversion(new GuidEnumConverter<OrganisationStateEnum>());
            //modelBuilder.Entity<DbTripRequest>().Property(d => d.State).HasConversion(new GuidEnumConverter<TripRequestStateEnum>());
            modelBuilder.Entity<DbStateMachineState>().Property(d => d.StateMachine).HasConversion(new GuidEnumConverter<StateMachineEnum>());
            modelBuilder.Entity<DbStateMachineAction>().Property(d => d.StateMachine).HasConversion(new GuidEnumConverter<StateMachineEnum>());
            modelBuilder.Entity<DbStateMachineFromStatus>().Property(d => d.StateMachine).HasConversion(new GuidEnumConverter<StateMachineEnum>());

            //modelBuilder.Entity<DbAccountRight>().HasAlternateKey(a => new { a.AccountId, a.RightId, a.OrganisationId });
            //modelBuilder.Entity<DbTripRequestIdentifier>().HasAlternateKey(a => new { a.TripRequestId });
            //modelBuilder.Entity<DbTripRequestIdentifier>().HasAlternateKey(a => new { a.Identifier });

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
