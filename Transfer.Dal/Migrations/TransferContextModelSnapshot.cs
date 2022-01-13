﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Transfer.Dal.Context;

namespace Transfer.Dal.Migrations
{
    [DbContext(typeof(TransferContext))]
    partial class TransferContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:IdentityIncrement", 1)
                .HasAnnotation("SqlServer:IdentitySeed", 1)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Transfer.Dal.Entities.DbAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbAccountRight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RightId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.HasIndex("RightId");

                    b.HasIndex("AccountId", "RightId", "OrganisationId")
                        .IsUnique()
                        .HasFilter("[RightId] IS NOT NULL AND [OrganisationId] IS NOT NULL");

                    b.ToTable("AccountRights");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbDriver", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TelegramId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbDriversLicense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DocumentCatigories")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("DocumentDateOfIssue")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DocumentEndDateOfIssue")
                        .HasColumnType("datetime2");

                    b.Property<string>("DocumentIssurer")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("DocumentNumber")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("DocumentSeries")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<Guid?>("DriverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.ToTable("DriverLicenses");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbExternalLogin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<Guid>("LoginType")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SubValue")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("ExternalLogins");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbOrganisation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("Checked")
                        .HasColumnType("bit");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("DirectorFio")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("DirectorPosition")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("INN")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("OGRN")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<Guid?>("RegionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbOrganisationAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountType")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("OrganisationId");

                    b.ToTable("OrganisationAccounts");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbOrganisationWorkingArea", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RegionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationId");

                    b.HasIndex("RegionId");

                    b.ToTable("OrganisationWorkingAreas");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbRegion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbRight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.ToTable("Rights");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbAccountRight", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbAccount", "Account")
                        .WithMany("AccountRights")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Transfer.Dal.Entities.DbOrganisation", "Organisation")
                        .WithMany()
                        .HasForeignKey("OrganisationId");

                    b.HasOne("Transfer.Dal.Entities.DbRight", "Right")
                        .WithMany("AccountRights")
                        .HasForeignKey("RightId");

                    b.Navigation("Account");

                    b.Navigation("Organisation");

                    b.Navigation("Right");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbDriver", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbOrganisation", "Organisation")
                        .WithMany("Drivers")
                        .HasForeignKey("OrganisationId");

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbDriversLicense", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbDriver", "Driver")
                        .WithMany("DbDriversLicenses")
                        .HasForeignKey("DriverId");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbExternalLogin", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbAccount", "Account")
                        .WithMany("ExternalLogins")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbOrganisation", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbRegion", "Region")
                        .WithMany("Organisations")
                        .HasForeignKey("RegionId");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbOrganisationAccount", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbAccount", "Account")
                        .WithMany("Organisations")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Transfer.Dal.Entities.DbOrganisation", "Organisation")
                        .WithMany("Accounts")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbOrganisationWorkingArea", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbOrganisation", "Organisation")
                        .WithMany("WorkingArea")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Transfer.Dal.Entities.DbRegion", "Region")
                        .WithMany("WorkingArea")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Organisation");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbAccount", b =>
                {
                    b.Navigation("AccountRights");

                    b.Navigation("ExternalLogins");

                    b.Navigation("Organisations");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbDriver", b =>
                {
                    b.Navigation("DbDriversLicenses");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbOrganisation", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Drivers");

                    b.Navigation("WorkingArea");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbRegion", b =>
                {
                    b.Navigation("Organisations");

                    b.Navigation("WorkingArea");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbRight", b =>
                {
                    b.Navigation("AccountRights");
                });
#pragma warning restore 612, 618
        }
    }
}
