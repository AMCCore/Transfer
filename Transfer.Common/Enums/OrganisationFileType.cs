using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums;

public enum OrganisationFileType
{
    /// <summary>
    /// Логотип компании
    /// </summary>
    [Description("Логотип компании")]
    [EnumGuid("448D23E0-3FAA-46E5-BD07-ACE51937850B")]
    Logo,

    /// <summary>
    /// Скан лицензии на осуществление перевозок
    /// </summary>
    [Description("Скан лицензии на осуществление перевозок")]
    [EnumGuid("2BDABBB7-49E3-423E-9DD8-25FF46D28342")]
    License,

}

