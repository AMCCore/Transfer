using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;

namespace Transfer.Dal.Entities
{
    [Table("TripOptions")]
    public class DbTripOption : IEntityBase, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; }

        [MaxLength(1000)]
        public string Name { get; set; }

        public virtual ICollection<DbTripRequestOption> TripRequests { get; set; }
    }
}