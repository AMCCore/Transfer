using System;
using System.ComponentModel.DataAnnotations;

namespace Transfer.Bl.Dto.Report;

public class InputDataReportDto
{
    /// <summary>
    /// Дата внесения
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime DateInput { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public string Manager { get; set; }

    /// <summary>
    /// Перевозчик
    /// </summary>
    public string Carrier { get; set; }

    /// <summary>
    /// Модель
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Марка
    /// </summary>
    public string Make { get; set; }

    /// <summary>
    /// Гос номер
    /// </summary>
    public string LicenseNumber { get; set; }

    /// <summary>
    /// Год выпуска
    /// </summary>
    public int Yaer { get; set; }

    /// <summary>
    /// Кол-во посадочных мест
    /// </summary>
    public int PeopleCopacity { get; set; }

    /// <summary>
    /// СТС
    /// </summary>
    public string? Reg { get; set; }

    /// <summary>
    /// ОСАГО
    /// </summary>
    public DateTime? OSAGOToDate { get; set; }

    /// <summary>
    /// Срок действия диагностической карты
    /// </summary>
    public DateTime? TOToDate { get; set; }

    /// <summary>
    /// Срок действия диагностической ОСГОП
    /// </summary>
    public DateTime? OSGOPToDate { get; set; }

    /// <summary>
    /// Кол-во фото
    /// </summary>
    public int FotoCount { get; set; }
}
