using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums
{
    public enum ExternalLoginTypeEnum
    {
        /// <summary>
        /// Telegram
        /// </summary>
        [Description("Telegram")]
        [EnumGuid("1B4D3A0C-D2AC-49D5-87BF-5EF81DF06524")]
        Telegram,

        /// <summary>
        /// Facebook
        /// </summary>
        [Description("Facebook")]
        [EnumGuid("52008941-A8CE-450A-9725-044ED4D0558E")]
        Facebook,


        /// <summary>
        /// Google
        /// </summary>
        [Description("Google")]
        [EnumGuid("5EC61BDB-CFCA-441B-AE0F-98F7C18D6C9D")]
        Google,

        /// <summary>
        /// AcceptCode
        /// </summary>
        [Description("AcceptCode")]
        [EnumGuid("0fdb9fda-5161-4fe6-97dc-812bb8b6e054")]
        AcceptCode,

        /// <summary>
        /// Devicetoken
        /// </summary>
        [Description("Devicetoken")]
        [EnumGuid("bcdf133d-4655-48e0-bccf-1c28b2d839cf")]
        Devicetoken,
    }
}
