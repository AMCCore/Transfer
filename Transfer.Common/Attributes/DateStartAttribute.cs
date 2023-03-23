using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common.Attributes;

public sealed class DateStartAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime dateStart = (DateTime)value;
        return (dateStart > DateTime.Now);
    }
}
