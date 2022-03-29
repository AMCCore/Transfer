using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.AccessRights
{
    public enum TripRequestRights
    {
        /// <summary>
        /// Полное всеобъемлющее право на любые действия с заявками на перевозки
        /// </summary>
        [Description("Полное всеобъемлющее право на любые действия с заявками на перевозки")]
        [EnumGuid("C81A8E61-FE4D-40CF-8A44-1F1EF5C1EF6A")]
        TripRequestAdmin,
    }
}
