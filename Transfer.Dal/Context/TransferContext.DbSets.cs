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
        public virtual DbSet<DbRight> DbRight { get; set; }
        public virtual DbSet<DbRegion> DdRegion { get; set; }
        public virtual DbSet<DbExternalLogin> DbExternalLogin { get; set; }
        public virtual DbSet<DbOrganisation> DbOrganisation { get; set; }
        public virtual DbSet<DbOrganisationWorkingArea> DbOrganisationWorkingArea { get; set; }
        public virtual DbSet<DbDriver> DbDriver { get; set; }
        public virtual DbSet<DbDriversLicense> DbDriversLicense { get; set; }
        public virtual DbSet<DbOrganisationAccount> DbOrganisationAccount { get; set; }
        public virtual DbSet<DbPersonData> DbPersonData { get; set; }
        public virtual DbSet<DbTripOption> DbTripOption { get; set; }
        public virtual DbSet<DbTripRequest> DbTripRequest { get; set; }
        public virtual DbSet<DbTripRequestOption> DbTripRequestOption { get; set; }
    }
}
