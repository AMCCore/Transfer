using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common
{
    public interface ISoftDeleteEntity
    {
        bool IsDeleted { get; set; }
    }
}
