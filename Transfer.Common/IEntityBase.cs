using System;

namespace Transfer.Common;

public interface IEntityBase
{
    Guid Id { get; set; }

    long LastUpdateTick { get; set; }
}
