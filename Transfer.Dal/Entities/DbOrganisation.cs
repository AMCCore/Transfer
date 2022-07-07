using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Enums.States;

namespace Transfer.Dal.Entities;

[Table("Organisations")]
public class DbOrganisation : IEntityBase, ISoftDeleteEntity
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

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

    public virtual ICollection<DbTripRequest> TripRequests { get; set; } = new List<DbTripRequest>();

    public OrganisationStateEnum State { get; set; } = OrganisationStateEnum.Checked;

    public virtual ICollection<DbTripRequestReplay> TripRequestReplays { get; set; } = new List<DbTripRequestReplay>();

    public virtual ICollection<DbTripRequestOffer> TripRequestOffers { get; set; } = new List<DbTripRequestOffer>();
}