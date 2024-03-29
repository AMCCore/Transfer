﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums.States;

namespace Transfer.Dal.Entities;

[Table("Organisations")]
public class DbOrganisation : IEntityBase, ISoftDeleteEntity, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; }

    [ForeignKey(nameof(Creator))]
    public Guid? CreatorId { get; set; }

    public virtual DbAccount? Creator { get; set; }

    /// <summary>
    ///     Наименование
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Name { get; set; }

    /// <summary>
    ///     Полное наименование
    /// </summary>
    [MaxLength(1000)]
    public string? FullName { get; set; }

    /// <summary>
    ///     ИНН
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string INN { get; set; }

    /// <summary>
    ///     ОГРН
    /// </summary>
    [MaxLength(1000)]
    public string? OGRN { get; set; }

    /// <summary>
    ///     Адрес
    /// </summary>
    [MaxLength(1000)]
    public string? Address { get; set; }

    /// <summary>
    ///     Город
    /// </summary>
    [MaxLength(1000)]
    public string? City { get; set; }

    /// <summary>
    ///     ФИО директора
    /// </summary>
    [MaxLength(1000)]
    public string DirectorFio { get; set; }

    /// <summary>
    ///     Должность директора
    /// </summary>
    [MaxLength(1000)]
    public string? DirectorPosition { get; set; }

    public virtual ICollection<DbOrganisationAccount> Accounts { get; set; } = new List<DbOrganisationAccount>();

    public virtual ICollection<DbOrganisationWorkingArea> WorkingArea { get; set; } = new List<DbOrganisationWorkingArea>();

    /// <summary>
    ///     Рэйтинг
    /// </summary>
    public double Rating { get; set; } = 0;

    /// <summary>
    ///     Организация проверена
    /// </summary>
    public bool Checked { get; set; }

    public virtual ICollection<DbOrganisationFile> Files { get; set; } = new List<DbOrganisationFile>();

    /// <summary>
    ///     Фактический адрес
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string FactAddress { get; set; }

    /// <summary>
    ///     Email
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Email { get; set; }

    /// <summary>
    ///     Телефон
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Phone { get; set; }

    /// <summary>
    ///     Согласие на оброаботку данных
    /// </summary>
    public bool Agreement { get; set; } = false;

    public virtual ICollection<DbBankDetails> BankDetails { get; set; } = new List<DbBankDetails>();

    public virtual ICollection<DbBus> Buses { get; set; } = new List<DbBus>();

    public virtual ICollection<DbDriver> Drivers { get; set; } = new List<DbDriver>();

    [InverseProperty(nameof(DbTripRequest.Charterer))]
    public virtual ICollection<DbTripRequest> TripRequestCharterers { get; set; } = new List<DbTripRequest>();

    [InverseProperty(nameof(DbTripRequest.OrgCreator))]
    public virtual ICollection<DbTripRequest> TripRequestCreators { get; set; } = new List<DbTripRequest>();

    public OrganisationStateEnum State { get; set; } = OrganisationStateEnum.Checked;

    public virtual ICollection<DbTripRequestReplay> TripRequestReplays { get; set; } = new List<DbTripRequestReplay>();

    public virtual ICollection<DbTripRequestOffer> TripRequestOffers { get; set; } = new List<DbTripRequestOffer>();

    /// <summary>
    ///     VIP
    /// </summary>
    [Required]
    [DefaultValue(false)]
    public bool IsVIP { get; set; } = false;

    /// <summary>
    ///     Агрегатор
    /// </summary>
    [Required]
    [DefaultValue(false)]
    public bool Agregator { get; set; } = false;

    /// <summary>
    ///     Признак активности перевозчика (Перевозчик активен нет/да)
    /// </summary>
    [Required]
    [DefaultValue(false)]
    public bool IsInactive { get; set; } = false;

}