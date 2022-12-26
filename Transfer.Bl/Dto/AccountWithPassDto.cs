using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto;

public class AccountWithPassDto : AccountDto
{
    public string Password { get; set; }
}
