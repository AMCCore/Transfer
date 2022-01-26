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
    [Required]
    public string BankName { get; set; }

    /// <summary>
    ///     БИК
    /// </summary>
    [MaxLength(100)]
    [Required]
    public string Bik { get; set; }

    /// <summary>
    ///     ИНН
    /// </summary>
    [MaxLength(100)]
    [Required]
    public string BankInn { get; set; }

    /// <summary>
    ///     КПП
    /// </summary>
    [MaxLength(100)]
    [Required]
    public string Kpp { get; set; }

    /// <summary>
    ///     Кор. счет
    /// </summary>
    [MaxLength(100)]
    [Required]
    public string KorAccount { get; set; }

    /// <summary>
    ///     Лицевой счет
    /// </summary>
    [MaxLength(100)]
    [Required]
    public string NumAccount { get; set; }

    /// <summary>
    ///     Наименование получателя
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string NameAccount { get; set; }

    public IFormFile? LogoFile { get; set; }

    public IFormFile? LicenceFile { get; set; }

    public Guid? LogoFileId { get; set; }

    public Guid? LicenceFileId { get; set; }
}

