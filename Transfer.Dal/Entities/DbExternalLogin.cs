using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums;

namespace Transfer.Dal.Entities
{
    [Table("ExternalLogins")]
    public class DbExternalLogin : IEntityBase, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(Account))]
        [Required]
        public Guid AccountId { get; set; }

        public virtual DbAccount Account { get; set; }

        public virtual ExternalLoginTypeEnum LoginType { get; set; }

        [MaxLength(1000)]
        public string Value { get; set; }

        [MaxLength(1000)]
        public string? SubValue { get; set; }
    }
}
