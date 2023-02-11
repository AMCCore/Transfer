using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("StateMachines")]
public class DbStateMachine : IEntityBase, ISoftDeleteEntity
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public bool IsDeleted { get; set; }

    /// <summary>
    /// Наименование машины
    /// </summary>
    [Display(Description = "Наименование машины")]
    [MaxLength(1000, ErrorMessage = "\"Наименование машины\" не может быть больше 1000 символов")]
    [Required(ErrorMessage = "\"Наименование машины\" не может быть пустым")]
    [DataMember(Name = "name", EmitDefaultValue = false)]
    public virtual string Name { get; set; }
}
