using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;

namespace Transfer.Dal.Entities
{
    [Table("PersonDatas")]
    public class DbPersonData : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [MaxLength(1000)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public bool IsMale { get; set; }

        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        [MaxLength(1000)] 
        public string? PlaceOfBirth { get; set; }

        /// <summary>
        /// Серия документа
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string DocumentSeries { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Код подразделения
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string DocumentSubDivisionCode { get; set; }

        /// <summary>
        /// Орган выдавший документ
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string DocumentIssurer { get; set; }

        /// <summary>
        /// Дата выдачи документа
        /// </summary>
        [Required]
        public DateTime DocumentDateOfIssue { get; set; }

        [MaxLength(1000)]
        [Required]
        public string RegistrationAddress { get; set; }

        [MaxLength(1000)]
        public string? RealAddress { get; set; }
        
        public virtual DbDriver? Driver { get; set; }
        
        [ForeignKey(nameof(Driver))]
        public Guid? DriverId { get; set; }

        public virtual DbAccount? Account { get; set; }
        
        [ForeignKey(nameof(Account))]
        public Guid? AccountId { get; set; }
    }
}
