using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bot.Dtos
{
    public struct UserStateDto
    {
        public string State { get; set; }

        public string StateParams { get; set; }

        public bool IsActive { get; set; }
    }
}
