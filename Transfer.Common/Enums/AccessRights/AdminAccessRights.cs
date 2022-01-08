using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.AccessRights
{
    public enum AdminAccessRights
    {
        /// <summary>
        /// Полное всеобъемлющее право на любые действия
        /// </summary>
        [Description("Полное всеобъемлющее право на любые действия")]
        [EnumGuid("ECF45E41-F532-49A3-84CE-58E7212BBCCC")]
        IsAdmin,
    }
}
