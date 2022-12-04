using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.Report;

public class TripRequestsReportDto
{
    /// <summary>
    /// Дата внесения
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime DateInput { get; set; }

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public long Identifier { get; set; }

    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public Guid TripRequestId { get; set; }

    /// <summary>
    /// Заказчик
    /// </summary>
    public string Requester { get; set; }

    /// <summary>
    /// Дата подачи
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime DateStart { get; set; }

    /// <summary>
    /// Адрес старта
    /// </summary>
    public string AddressStart { get; set; }

    /// <summary>
    /// Адрес назанчения
    /// </summary>
    public string AddressFinish { get; set; }

    /// <summary>
    /// Цена
    /// </summary>
    [DataType(DataType.Currency)]
    public decimal Offer { get; set; }

    /// <summary>
    /// Кол-во посадочных мест
    /// </summary>
    public int PeopleCopacity { get; set; }

    /// <summary>
    /// Детская поездка
    /// </summary>
    public bool ChildTrip { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public string Manager { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public string State { get; set; }
}
