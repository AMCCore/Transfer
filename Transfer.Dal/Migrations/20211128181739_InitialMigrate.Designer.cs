﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Transfer.Dal.Context;

namespace Transfer.Dal.Migrations
{
    [DbContext(typeof(TransferContext))]
    [Migration("20211128181739_InitialMigrate")]
    partial class InitialMigrate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:IdentityIncrement", 1)
                .HasAnnotation("SqlServer:IdentitySeed", 1)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Transfer.Dal.Entities.DbAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

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

                    b.Property<Guid?>("PersonDataId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PersonDataId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbAccountRight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("OrganisationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RightId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RightId");

                    b.HasIndex("AccountId", "RightId", "OrganisationId")
                        .IsUnique()
                        .HasFilter("[OrganisationId] IS NOT NULL");

                    b.ToTable("AccountRights");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("FullAddress")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<Guid?>("RegionId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbExternalLogin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<Guid>("LoginType")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SubValue")
                        .IsRequired()
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

            modelBuilder.Entity("Transfer.Dal.Entities.DbPersonData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DocumentDateOfIssue")
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

                    b.Property<string>("DocumentSubDivisionCode")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsMale")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<long>("LastUpdateTick")
                        .HasColumnType("bigint");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("PlaceOfBirth")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<Guid?>("RealAddressId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RegistrationAddressId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RealAddressId");

                    b.HasIndex("RegistrationAddressId");

                    b.ToTable("PersonDatas");
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

            modelBuilder.Entity("Transfer.Dal.Entities.DdRegion", b =>
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

            modelBuilder.Entity("Transfer.Dal.Entities.DbAccount", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbPersonData", "PersonData")
                        .WithMany("Accounts")
                        .HasForeignKey("PersonDataId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("PersonData");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbAccountRight", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbAccount", "Account")
                        .WithMany("AccountRights")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Transfer.Dal.Entities.DbRight", "Right")
                        .WithMany("AccountRights")
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Right");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbAddress", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DdRegion", "Region")
                        .WithMany("DbAddresses")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Region");
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

            modelBuilder.Entity("Transfer.Dal.Entities.DbPersonData", b =>
                {
                    b.HasOne("Transfer.Dal.Entities.DbAddress", "RealAddress")
                        .WithMany("PersonDataRealAddress")
                        .HasForeignKey("RealAddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Transfer.Dal.Entities.DbAddress", "RegistrationAddress")
                        .WithMany("PersonDataRegistrations")
                        .HasForeignKey("RegistrationAddressId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("RealAddress");

                    b.Navigation("RegistrationAddress");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbAccount", b =>
                {
                    b.Navigation("AccountRights");

                    b.Navigation("ExternalLogins");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbAddress", b =>
                {
                    b.Navigation("PersonDataRealAddress");

                    b.Navigation("PersonDataRegistrations");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbPersonData", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DbRight", b =>
                {
                    b.Navigation("AccountRights");
                });

            modelBuilder.Entity("Transfer.Dal.Entities.DdRegion", b =>
                {
                    b.Navigation("DbAddresses");
                });
#pragma warning restore 612, 618
        }
    }
}
