using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("BankDetails")]
public class DbBankDetails : IEntityBase, ISoftDeleteEntity
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    /// <summary>
    ///     Наименование банка
    /// </summary>
    [MaxLength(1000)]
    public string? BankName { get; set; }

    /// <summary>
    ///     БИК
    /// </summary>
    [MaxLength(100)]
    public string? Bik { get; set; }

    /// <summary>
    ///     ИНН
    /// </summary>
    [MaxLength(100)]
    public string? Inn { get; set; }

    /// <summary>
    ///     КПП
    /// </summary>
    [MaxLength(100)]
    public string? Kpp { get; set; }

    /// <summary>
    ///     Кор. счет
    /// </summary>
    [MaxLength(100)]
    public string? KorAccount { get; set; }

    /// <summary>
    ///     Лицевой счет
    /// </summary>
    [MaxLength(100)]
    public string? NumAccount { get; set; }

    /// <summary>
    ///     Наименование получателя
    /// </summary>
    [MaxLength(1000)]
    public string? NameAccount { get; set; }

    public virtual DbOrganisation? Organisation { get; set; }

    [ForeignKey(nameof(Organisation))]
    public Guid? OrganisationId { get; set; }
}