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
    [Table("Regions")]
    public class DdRegion : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        [MaxLength(1000)]
        public string Name { get; set; }

        public virtual ICollection<DbAddress> DbAddresses { get; set; } = new List<DbAddress>();
    }
}
