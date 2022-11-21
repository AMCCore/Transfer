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
}
