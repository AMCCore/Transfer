using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.Bus
{
    public class BusDto
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
        public int PeopleCopacity { get; set; }


        /// <summary>
        /// Объём багажа
        /// </summary>
        public int? LuggageVolume { get; set; }

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
        /// Наличие аудиостсиемы
        /// </summary>
        public bool Audio { get; set; }

        /// <summary>
        /// Наличие туалета
        /// </summary>
        public bool WC { get; set; }

        /// <summary>
        /// Наличие микрофона
        /// </summary>
        public bool Microphone { get; set; }

        /// <summary>
        /// Наличие WiFi
        /// </summary>
        public bool Wifi { get; set; }

        public Guid? OsagoFileId { get; set; }
    }
}
