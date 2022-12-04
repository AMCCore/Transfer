using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Dal.Entities;

namespace Transfer.Dal.Context;

public partial class TransferContext
{
    public virtual DbSet<DbAccount> DbAccounts { get; set; }
    public virtual DbSet<DbAccountRight> DbAccountRights { get; set; }
    public virtual DbSet<DbBankDetails> DbBankDetails { get; set; }
    public virtual DbSet<DbBus> DbBuses { get; set; }
    public virtual DbSet<DbBusFile> DbBusFiles { get; set; }
    public virtual DbSet<DbDriver> DbDrivers { get; set; }
    public virtual DbSet<DbDriverFile> DbDriverFiles { get; set; }
    public virtual DbSet<DbDriversLicense> DbDriversLicense { get; set; }
    public virtual DbSet<DbExternalLogin> DbExternalLogins { get; set; }
    public virtual DbSet<DbFile> DbFiles { get; set; }
    public virtual DbSet<DbHistoryLog> DbHistoryLogs { get; set; }
    public virtual DbSet<DbOrganisation> DbOrganisations { get; set; }
    public virtual DbSet<DbOrganisationAccount> DbOrganisationAccounts { get; set; }
    public virtual DbSet<DbOrganisationFile> DbOrganisationFiles { get; set; }
    public virtual DbSet<DbOrganisationWorkingArea> DbOrganisationWorkingAreas { get; set; }
    public virtual DbSet<DbPersonData> DbPersonDatas { get; set; }
    public virtual DbSet<DbRight> DbRights { get; set; }
    public virtual DbSet<DbStateMachineState> DbStateMachineStates { get; set; }
    public virtual DbSet<DbStateMachineStateRight> DbStateMachineStateRights { get; set; }
    public virtual DbSet<DbTgActionState> DbTgActionStates { get; set; }
    public virtual DbSet<DbTripOption> DbTripOptions { get; set; }
    public virtual DbSet<DbTripRequest> DbTripRequests { get; set; }
    public virtual DbSet<DbTripRequestOption> DbTripRequestOptions { get; set; }
    public virtual DbSet<DbTripRequestReplay> DbTripRequestReplays { get; set; }
    public virtual DbSet<DbTripRequestOffer> DbTripRequestOffers { get; set; }
    public virtual DbSet<DbRegion> DdRegions { get; set; }
}

