using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;

namespace Transfer.Dal.Entities
{
    [Table("DriverLicenses")]
    public class DbDriversLicense : IEntityBase, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        /// <summary>
        /// Серия документа
        /// </summary>
        [MaxLength(1000)]
        public string DocumentSeries { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [MaxLength(1000)]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Дата выдачи документа
        /// </summary>
        public DateTime DocumentDateOfIssue { get; set; }

        /// <summary>
        /// Дата выдачи документа
        /// </summary>
        public DateTime DocumentEndDateOfIssue { get; set; }

        /// <summary>
        /// Орган выдавший документ
        /// </summary>
        [MaxLength(1000)]
        public string DocumentIssurer { get; set; }

        /// <summary>
        /// Категории транспортных средств (через запятую)
        /// </summary>
        [MaxLength(1000)]
        public string DocumentCatigories { get; set; }
        
        public virtual DbDriver? Driver { get; set; }
        
        [ForeignKey(nameof(Driver))]
        public Guid? DriverId { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}
