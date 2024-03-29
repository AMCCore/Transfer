﻿using System;
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

        /// <summary>
        /// Текущий статус (текущее состояние)
        /// </summary>
        public abstract Guid State { get; set; }

        public bool NeedSaveButton { get; set; } = true;
    }
}
