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
    [Table("Organisations")]
    public class DbOrganisation : IEntityBase, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        ///     Наименование
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Полное наименование
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string FullName { get; set; }

        /// <summary>
        ///     ИНН
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string INN { get; set; }

        /// <summary>
        ///     ОГРН
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string OGRN { get; set; }

        /// <summary>
        ///     Адрес
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string Address { get; set; }

        /// <summary>
        ///     Регион
        /// </summary>
        [ForeignKey(nameof(Region))]
        public Guid? RegionId { get; set; }

        /// <summary>
        ///     Регион
        /// </summary>
        public virtual DbRegion Region { get; set; }

        /// <summary>
        ///     Город
        /// </summary>
        [MaxLength(1000)] 
        public string City { get; set; }

        /// <summary>
        ///     ФИО директора
        /// </summary>
        [MaxLength(1000)]
        public string DirectorFio { get; set; }

        /// <summary>
        ///     Должность директора
        /// </summary>
        [MaxLength(1000)]
        public string? DirectorPosition { get; set; }

        public virtual ICollection<DbAccount> Accounts { get; set; } = new List<DbAccount>();

        public virtual ICollection<DbOrganisationWorkingArea> WorkingArea { get; set; } = new List<DbOrganisationWorkingArea>();
    }
}
