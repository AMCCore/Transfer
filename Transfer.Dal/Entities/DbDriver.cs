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
        
        public virtual DbOrganisation? Organisation { get; set; }
        
        [ForeignKey(nameof(Organisation))]
        public Guid? OrganisationId { get; set; }

        public string? TelegramId { get; set; }
        
        public virtual ICollection<DbDriversLicense> DbDriversLicenses { get; set; } = new List<DbDriversLicense>();

        public virtual DbPersonData? PersonData { get; set; }

        [ForeignKey(nameof(PersonData))]
        public Guid? PersonDataId { get; set; }
    }
}
