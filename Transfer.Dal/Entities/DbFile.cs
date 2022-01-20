using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;

namespace Transfer.Dal.Entities;

[Table("Files")]
public class DbFile : IEntityBase, IEntityWithDateCreated
{
    [Key]
    public Guid Id { get; set; }

    public long LastUpdateTick { get; set; }
    
    public DateTime DateCreated { get; set; }

    [MaxLength(10)]
    public string Extention { get; set; }

    public long? Size { get; set; }

    [MaxLength(1000)]
    public string? ContentType { get; set; }
}

