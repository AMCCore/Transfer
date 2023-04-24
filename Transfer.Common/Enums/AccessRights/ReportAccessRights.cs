using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.AccessRights;

public enum ReportAccessRights
{
    /// <summary>
    /// Отчет о внесении перевозчиков
    /// </summary>
    [Description("Отчет о внесении перевозчиков")]
    [EnumGuid("65CAE23E-2057-40EA-8967-BEFE0CF8E41F")]
    DataInputReport,

    /// <summary>
    /// Отчет о внесении запросов на перевозки
    /// </summary>
    [Description("Отчет о внесении запросов на перевозки")]
    [EnumGuid("C8567735-785F-4416-9509-AAEC72AFC2EB")]
    TripRequestReport,

    /// <summary>
    /// Отчет о внесении ТС
    /// </summary>
    [Description("Отчет о внесении ТС")]
    [EnumGuid("f30bca8b-749a-47b6-a96d-7883aca2ac6b")]
    DataTransportInputReport,
}
