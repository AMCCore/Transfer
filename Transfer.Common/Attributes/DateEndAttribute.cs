using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common.Attributes;

public sealed class DateEndAttribute : ValidationAttribute
{
    public string DateStartProperty { get; set; }

    //в разработке
    public override bool IsValid(object value)
    {
        return false;

        // Get Value of the DateStart property
        //string dateStartString = HttpContext.Current.Request[DateStartProperty];
        //DateTime dateEnd = (DateTime)value;
        //DateTime dateStart = DateTime.Parse(dateStartString);

        //// Meeting start time must be before the end time
        //return dateStart < dateEnd;
    }
}
