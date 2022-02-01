using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto;
public class OrganisationDto
{
    public Guid Id { get; set; } = Guid.Empty;

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
    ///     Город
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string City { get; set; }

    /// <summary>
    ///     ФИО директора
    /// </summary>
    [MaxLength(1000)]
    public string? ContactFio { get; set; }

    /// <summary>
    ///     Рэйтинг
    /// </summary>
    public double Rating { get; set; } = 0;

    /// <summary>
    ///     Организация проверена
    /// </summary>
    public bool Checked { get; set; }

    /// <summary>
    ///     Фактический адрес
    /// </summary>
    [MaxLength(1000)]
    public string? FactAddress { get; set; }

    /// <summary>
    ///     Email
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Email { get; set; }

    /// <summary>
    ///     Телефон
    /// </summary>
    [MaxLength(1000)]
    [Required]
    public string Phone { get; set; }

    /// <summary>
    ///     Согласие на оброаботку данных
    /// </summary>
    public bool Agreement { get; set; } = false;
}

