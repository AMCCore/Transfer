using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Dal.Entities;

namespace Transfer.Dal.Context
{
    public partial class TransferContext
    {
        public virtual DbSet<DbAccount> DbAccount { get; set; }
        public virtual DbSet<DbAccountRight> DbAccountRight { get; set; }
        public virtual DbSet<DbAddress> DbAddress { get; set; }
        public virtual DbSet<DbPersonData> DbPersonData { get; set; }
        public virtual DbSet<DbRight> DbRight { get; set; }
        public virtual DbSet<DdRegion> DdRegion { get; set; }
        public virtual DbSet<DbExternalLogin> DbExternalLogin { get; set; }
    }
}
