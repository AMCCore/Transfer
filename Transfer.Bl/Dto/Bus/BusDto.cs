using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.Bus
{
    public class BusDto : StateMachineDto
    {
        public Guid Id { get; set; }

        public long LastUpdateTick { get; set; }

        public bool IsDeleted { get; set; }

        public Guid OrganisationId { get; set; }

        public string? OrganisationName { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Model { get; set; }

        /// <summary>
        /// Марка
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string Make { get; set; }

        /// <summary>
        /// Гос номер
        /// </summary>
        [MaxLength(1000)]
        [Required]
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Год выпуска
        /// </summary>
        [Required]
        public int Yaer { get; set; }

        /// <summary>
        /// Кол-во посадочных мест
        /// </summary>
        [Required]
        [Range(1, 1000)]
        public int PeopleCopacity { get; set; }

        /// <summary>
        /// Наличие телевизора
        /// </summary>
        public bool TV { get; set; }

        /// <summary>
        /// Наличие кондиционера
        /// </summary>
        public bool AirConditioner { get; set; }

        /// <summary>
        /// Наличие ремней безопасности
        /// </summary>
        public bool SaftyBelts { get; set; }

        /// <summary>
        /// Наличие туалета
        /// </summary>
        public bool WC { get; set; }

        /// <summary>
        /// Наличие микрофона
        /// </summary>
        public bool Microphone { get; set; }

        public Guid? OsagoFileId { get; set; }

        public Guid? RegFileId { get; set; }

        public Guid? ToFileId { get; set; }

        public Guid? OsgopFileId { get; set; }

        public Guid? TahografFileId { get; set; }

        public Guid? Photo1 { get; set; }

        public Guid? Photo2 { get; set; }

        public Guid? Photo3 { get; set; }

        public Guid? Photo4 { get; set; }

        public Guid? Photo5 { get; set; }

        public Guid? Photo6 { get; set; }

        /// <summary>
        /// Серия СТС
        /// </summary>
        public string? RegSeries { get; set; }

        /// <summary>
        /// Номер СТС
        /// </summary>
        public string? RegNumber { get; set; }

        /// <summary>
        /// Серия ОСАГО
        /// </summary>
        public string? OSAGOSeries { get; set; }

        /// <summary>
        /// Номер ОСАГО
        /// </summary>
        public string? OSAGONumber { get; set; }

        /// <summary>
        /// Срок действия диагностической карты
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Номер диагностической карты
        /// </summary>
        public string? ToNumber { get; set; }

        [Required]
        public override Guid State { get; set; }
    }

}
