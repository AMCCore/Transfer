using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common.Extensions;

public static class GuidExtension
{
    public static bool IsNullOrEmpty(this Guid? g)
    {
        return !g.HasValue || g.Value == Guid.Empty;
    }

    public static bool IsNullOrEmpty(this Guid g)
    {
        return g == Guid.Empty;
    }
}
