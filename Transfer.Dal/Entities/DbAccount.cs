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
    [Table("Accounts")]
    public class DbAccount : IEntityBase, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Email { get; set; }

        [MaxLength(1000)]
        public string Password { get; set; }

        [ForeignKey(nameof(PersonData))]
        public Guid? PersonDataId { get; set; }

        public virtual DbPersonData PersonData { get; set; }

        public virtual ICollection<DbAccountRight> AccountRights { get; set; } = new List<DbAccountRight>();

        public virtual ICollection<DbExternalLogin> ExternalLogins { get; set; } = new List<DbExternalLogin>();
    }
}
