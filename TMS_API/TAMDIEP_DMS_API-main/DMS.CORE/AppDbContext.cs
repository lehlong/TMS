using DMS.CORE.Common;
using DMS.CORE.Entities.AD;
using DMS.CORE.Entities.BU;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MN;
using DMS.CORE.Entities.SO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DMS.CORE
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeleteEntity).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }

            modelBuilder.HasSequence<int>("ORDER_SEQUENCE")
                    .StartsAt(1)
                    .IncrementsBy(1);

            base.OnModelCreating(modelBuilder);
        }

        public Func<DateTime> TimestampProvider { get; set; } = ()
            => DateTime.Now;

        public override int SaveChanges()
        {
            TrackChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TrackChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void TrackChanges()
        {
            var tokens = _httpContextAccessor?.HttpContext?.Request?.Headers.Authorization.ToString()?.Split(" ")?.ToList();
            string? user = null;
            if (tokens != null)
            {
                var token = tokens.FirstOrDefault(x => x != "Bearer");
                if (!string.IsNullOrWhiteSpace(token) && token != "null")
                {
                    JwtSecurityTokenHandler tokenHandler = new();
                    JwtSecurityToken securityToken =  tokenHandler.ReadToken(token) as JwtSecurityToken;
                    var claim = securityToken.Claims;
                    var result = claim.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                    user = result?.Value;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IBaseEntity auditable)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreateBy = user;
                        auditable.CreateDate = TimestampProvider();
                    }
                    else
                    {
                        Entry(auditable).Property(x => x.CreateBy).IsModified = false;
                        Entry(auditable).Property(x => x.CreateDate).IsModified = false;
                        auditable.UpdateBy = user;
                        auditable.UpdateDate = TimestampProvider();
                    }
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
            {
                if (entry.Entity is ISoftDeleteEntity deletedEntity)
                {
                    entry.State = EntityState.Unchanged;
                    deletedEntity.IsDeleted = true;
                    deletedEntity.DeleteBy = user;
                    deletedEntity.DeleteDate = TimestampProvider();
                }
            }
        }

        public async Task<int> GetNextSequenceValue(string sequence)
        {
            using var command = Database.GetDbConnection().CreateCommand();
            command.CommandText = $"SELECT {sequence}.NEXTVAL FROM DUAL";
            await Database.OpenConnectionAsync();
            using var result = await command.ExecuteReaderAsync();
            await result.ReadAsync();
            return result.GetInt32(0);
        }

        #region System Manage
        public DbSet<TblAdAccount> TblAdAccount { get; set; }
        public DbSet<TblAdAccountGroup> TblAdAccountGroup { get; set; }
        public DbSet<TblAdMenu> TblAdMenu { get; set; }
        public DbSet<TblAdRight> TblAdRight { get; set; }
        public DbSet<TblAdMessage> TblAdMessage { get; set; }
        public DbSet<TblAdAccountGroupRight> TblAdAccountGroupRight { get; set; }
        public DbSet<TblAdAccountRefreshToken> TblAdAccountRefreshToken { get; set; }
        public DbSet<TblAdAppVersion> TblAdAppVersion { get; set; }
        public DbSet<TblAdAccount_AccountGroup> TblAdAccount_AccountGroup { get; set; }
        public DbSet<TblActionLog> TblActionLogs {get; set;}
        public DbSet<TblAdSystemTrace> TblAdSystemTrace { get; set; }
        #endregion

        #region Master Data
        public DbSet<TblMdProvine> TblMdProvine { get; set; }
        public DbSet<TblMdDistrict> TblMdDistrict { get; set; }
        public DbSet<TblMdWard> TblMdWard { get; set; }
        public DbSet<TblMdGroupItem> TblMdGroupItem { get; set; }
        public DbSet<TblMdTypeItem> TblMdTypeItem { get; set; }
        public DbSet<TblMdUnit> TblMdUnit { get; set; }
        public DbSet<TblMdPartner> TblMdPartner { get; set; }
        public DbSet<TblMdVehicle> TblMdVehicle { get; set; }
        public DbSet<TblMdSaleType> TblMdSaleType { get; set; }
        public DbSet<TblMdVehicleType> TblMdVehicleType { get; set; }
        public DbSet<TblMdItem> TblMdItem { get; set; }
        public DbSet<TblMdArea> TblMdArea { get; set; }
        public DbSet<TblMdDriver> TblMdDriver { get; set; }

        #endregion

        #region Manage
        public DbSet<TblMnPartnerReference> TblMnPartnerReference { get; set; }
        public DbSet<TblMnPartnerVehicle> TblMnPartnerVehicle { get; set; }
        public DbSet<TblMnAreaItem> TblMnAreaItem { get; set; }
        public DbSet<TblMnAccountSaleOffice> TblMnAccountSaleOffices { get; set; }
        public DbSet<TblMnAccountPlanVisit> TblMnAccountPlanVisits { get; set; }
        public DbSet<TblMnAccountPlanVisitStore> TblMnAccountPlanVisitStores { get; set; }
        public DbSet<TblMnAccountCareStore> TblMnAccountCareStores { get; set; }
        public DbSet<TblMnPartnerContract> TblMnPartnerContract { get; set; }
        public DbSet<TblMnContract> TblMnContract { get; set; }

        #endregion

        #region Common
        public DbSet<TblCmAttachment> TblBuAttachment { get; set; }
        public DbSet<TblCmModuleAttachment> TblBuModuleAttachment { get; set; }
        public DbSet<TblCmComment> TblCmComment { get; set; }
        public DbSet<TblCmModuleComment> TblCmModuleComment { get; set; }

        #endregion

        #region Sale Order
        public DbSet<TblSoOrder> TblSoOrder { get; set; }
        public DbSet<TblSoOrderDetail> TblSoOrderDetail { get; set; }
        public DbSet<TblSoOrderProcess> TblSoOrderProcess { get; set; }
        #endregion
    }
}
