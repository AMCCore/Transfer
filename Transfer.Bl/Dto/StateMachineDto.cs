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

        public IDictionary<Guid, string> NextStates { get; set; } = new Dictionary<Guid, string>();

        public abstract Guid State { get; set; }

        public bool NeedSaveButton { get; set; } = true;
    }
}
