using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Bl.Dto
{
    public abstract class StateMachineDto
    {
        public Guid? StateChange { get; set; }

        public ICollection<NextStateDto> NextStates { get; set; } = new List<NextStateDto>();

        public abstract Guid State { get; set; }
    }
}
