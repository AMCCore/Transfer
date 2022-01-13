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
    [Table("Drivers")]
    public class DbDriver : IEntityBase, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(DriversLicense))]
        public Guid? DbDriversLicenseId { get; set; }

        public virtual DbDriversLicense DriversLicense { get; set; }

        public string? TelegramId { get; set; }
        
        [ForeignKey(nameof(Organisation))]
        [Required]
        public Guid? OrganisationId { get; set; }

        public virtual DbOrganisation Organisation { get; set; }
    }
}
