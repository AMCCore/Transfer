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
    [Table("AccountRights")]
    public class DbAccountRight : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        /// <summary>
        ///     Право пользователя
        /// </summary>
        [ForeignKey(nameof(Right))]
        public Guid? RightId { get; set; }

        /// <summary>
        ///     Право пользователя
        /// </summary>
        public virtual DbRight? Right { get; set; }

        /// <summary>
        ///     УЗ пользователя
        /// </summary>
        [ForeignKey(nameof(Account))]
        [Required]
        public Guid AccountId { get; set; }

        /// <summary>
        ///     УЗ пользователя
        /// </summary>
        public virtual DbAccount Account { get; set; }

        /// <summary>
        ///     Организация
        /// </summary>
        [ForeignKey(nameof(Organisation))]
        public Guid? OrganisationId { get; set; }
        
        /// <summary>
        ///     УЗ пользователя
        /// </summary>
        public virtual DbOrganisation? Organisation { get; set; }

    }
}
