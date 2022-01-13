using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transfer.Common;
using Transfer.Common.Enums;

namespace Transfer.Dal.Entities
{
    [Table("OrganisationAccounts")]
    public class DbOrganisationAccount : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        [Required]
        public OrganisationAccountType AccountType { get; set; } = OrganisationAccountType.Operator;
        
        [ForeignKey(nameof(Organisation))]
        [Required]
        public Guid OrganisationId { get; set; }

        public virtual DbOrganisation Organisation { get; set; }
        
        [ForeignKey(nameof(Account))]
        [Required]
        public Guid AccountId { get; set; }

        public virtual DbAccount Account { get; set; }
    }
}