using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Extensions;

namespace Transfer.Dal.Entities
{
    [Table("Rights")]
    public class DbRight : IEntityBase, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<DbAccountRight> AccountRights { get; set; } = new List<DbAccountRight>();

        //public virtual ICollection<DbStateMachineStateRight> StateMachineStateRights { get; set; } = new List<DbStateMachineStateRight>();

        internal static DbRight CreateForSeed(Enum right, bool isDeleted = false)
        {
            return new DbRight
            {
                Id = right.GetEnumGuid(),
                Name = right.GetEnumDescription(),
                IsDeleted = isDeleted
            };
        }
    }
}
