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

[Table("Buses")]
public class DbBus : IEntityBase, ISoftDeleteEntity, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual DbOrganisation? Organisation { get; set; }

    [ForeignKey(nameof(Organisation))]
    public Guid? OrganisationId { get; set; }

    public virtual ICollection<DbBusFile> BusFiles { get; set; } = new List<DbBusFile>();

    /// <summary>
    /// Модель
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Model { get; set; }

    /// <summary>
    /// Марка
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Make { get; set; }

    /// <summary>
    /// Гос номер
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string LicenseNumber { get; set; }

    /// <summary>
    /// Год выпуска
    /// </summary>
    [Required]
    public int Yaer { get; set; }

    /// <summary>
    /// Кол-во посадочных мест
    /// </summary>
    [Required]
    public int PeopleCopacity { get; set; }


    /// <summary>
    /// Объём багажа
    /// </summary>
    public int? LuggageVolume { get; set; }

    /// <summary>
    /// Наличие телевизора
    /// </summary>
    public bool TV { get; set; }

    /// <summary>
    /// Наличие кондиционера
    /// </summary>
    public bool AirConditioner { get; set; }

    /// <summary>
    /// Наличие ремней безопасности
    /// </summary>
    public bool SaftyBelts { get; set; }

    /// <summary>
    /// Наличие аудиостсиемы
    /// </summary>
    public bool Audio { get; set; }

    /// <summary>
    /// Наличие туалета
    /// </summary>
    public bool WC { get; set; }

    /// <summary>
    /// Наличие микрофона
    /// </summary>
    public bool Microphone { get; set; }

    /// <summary>
    /// Наличие WiFi
    /// </summary>
    public bool Wifi { get; set; }

    /// <summary>
    /// Серия СТС
    /// </summary>
    public string? RegSeries { get; set; }

    /// <summary>
    /// Номер СТС
    /// </summary>
    public string? RegNumber { get; set; }

    /// <summary>
    /// Серия ОСАГО
    /// </summary>
    public string? OSAGOSeries { get; set; }

    /// <summary>
    /// Номер ОСАГО
    /// </summary>
    public string? OSAGONumber { get; set; }

    /// <summary>
    /// Срок действия диагностической карты
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Номер диагностической карты
    /// </summary>
    public string? ToNumber { get; set; }

    public BusStateEnum State { get; set; } = BusStateEnum.Checked;
}

