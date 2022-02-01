using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Common.Enums;

namespace Transfer.Dal.Entities
{
    [Table("DriverFiles")]
    public class DbDriverFile : IEntityBase, IEntityWithDateCreated, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }

        public DriverFileType FileType { get; set; }

        [ForeignKey(nameof(Uploader))]
        [Required]
        public Guid UploaderId { get; set; }

        public virtual DbAccount Uploader { get; set; }

        [ForeignKey(nameof(Driver))]
        [Required]
        public Guid DriverId { get; set; }

        public virtual DbDriver Driver { get; set; }

        [ForeignKey(nameof(File))]
        [Required]
        public Guid FileId { get; set; }

        public virtual DbFile File { get; set; }
    }
}
