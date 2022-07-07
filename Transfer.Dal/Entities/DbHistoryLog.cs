﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("HistoryLogs")]
public class DbHistoryLog : IEntityBase, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }

    public DateTime DateCreated { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ActionName { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Value { get; set; }
}