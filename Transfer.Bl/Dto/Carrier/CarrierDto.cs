using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.Carrier;

public class CarrierDto : OrganisationDto
{
    /// <summary>
    ///     Наименование банка
    /// </summary>
    [MaxLength(1000)]
    public string? BankName { get; set; }

    /// <summary>
    ///     БИК
    /// </summary>
    [MaxLength(100)]
    public string? Bik { get; set; }

    /// <summary>
    ///     Лицевой счет
    /// </summary>
    [MaxLength(100)]
    public string? NumAccount { get; set; }

    public Guid? LogoFileId { get; set; }

    public Guid? LicenceFileId { get; set; }

    public string[] WorkingAreas { get; set; }
}

