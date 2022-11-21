using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto.Report;

public class BaseReportDto
{
    [DataType(DataType.Date)]
    public DateTime? DateFrom { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateTo { get; set; }

    public string Name { get; set; }

    public string Action { get; set; }

    public bool AsFile { get; set; } = false;
}
