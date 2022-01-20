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
    [EnumGuid("87384A8C-DAFD-4CF1-AB55-8E8315A122BA")]
    Logo,

    /// <summary>
    /// Скан лицензии на осуществление перевозок
    /// </summary>
    [Description("Скан лицензии на осуществление перевозок")]
    [EnumGuid("87384A8C-DAFD-4CF1-AB55-8E8315A122BA")]
    License,

}

