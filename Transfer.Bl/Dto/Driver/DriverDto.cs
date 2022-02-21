using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.Driver
{
    public class DriverDto
    {
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? OrganisationId { get; set; }

        public string? OrganisationName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string? MiddleName { get; set; }

        public string EMail { get; set; }

        public string Phone { get; set; }

        public Guid? Avatar { get; set; }

        public Guid? License1 { get; set; }

        public Guid? License2 { get; set; }

        public Guid? TahografFileId { get; set; }
    }
}
