using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common.Attributes
{
    public class EnumGuidAttribute : Attribute
    {
        public Guid Guid;

        public EnumGuidAttribute(string guid)
        {
            Guid = new Guid(guid);
        }
    }
}
