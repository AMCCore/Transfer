using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums
{
    public enum ExternalLoginEnum
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
    }
}
