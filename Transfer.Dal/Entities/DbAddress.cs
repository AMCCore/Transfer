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
    [Table("Addresses")]
    public class DbAddress : IEntityBase, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; }

        [InverseProperty("RegistrationAddress")]
        public virtual ICollection<DbPersonData> PersonDataRegistrations { get; set; } = new List<DbPersonData>();

        [InverseProperty("RealAddress")]
        public virtual ICollection<DbPersonData> PersonDataRealAddress { get; set; } = new List<DbPersonData>();


        /// <summary>
        /// Полный адресс (без индекса и населённого пункта)
        /// </summary>
        [MaxLength(1000)]
        public string FullAddress { get; set; }

        [MaxLength(6)]
        public string PostalCode { get; set; }

        [MaxLength(1000)]
        public string City { get; set; }

        [ForeignKey(nameof(Region))]
        public Guid? RegionId { get; set; }

        public virtual DdRegion Region { get; set; }
    }
}
