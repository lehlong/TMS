using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace XHTD.CORE.Entities
{
    //Update database
    //Scaffold-DbContext "Server=42.1.65.237;Database=TAMDIEP_XHTD;User ID=xhtd;Password=BUQTSX@2023; TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -Context XHTDDbContext -NoOnConfiguring -f

    public partial class XHTDDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public XHTDDbContext(DbContextOptions<XHTDDbContext> options, IHttpContextAccessor httpContextAccessor)
         : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
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
                    JwtSecurityToken securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                    var claim = securityToken.Claims;
                    var result = claim.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                    user = result?.Value;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                var entity = entry.Entity;
                var entityType = entity.GetType();

                var createdDateProperty = entityType.GetProperty("CreateDay");
                var createdByProperty = entityType.GetProperty("CreateBy");
                var updatedDateProperty = entityType.GetProperty("UpdateDay");
                var updatedByProperty = entityType.GetProperty("UpdateBy");

                if (entry.State == EntityState.Added)
                {
                    if (createdDateProperty != null && createdDateProperty.PropertyType == typeof(DateTime))
                    {
                        createdDateProperty.SetValue(entity, DateTime.Now);
                    }

                    if (createdByProperty != null && createdByProperty.PropertyType == typeof(string))
                    {
                        createdByProperty.SetValue(entity, user);
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (updatedDateProperty != null && updatedDateProperty.PropertyType == typeof(DateTime))
                    {
                        updatedDateProperty.SetValue(entity, DateTime.Now);
                    }

                    if (updatedByProperty != null && updatedByProperty.PropertyType == typeof(string))
                    {
                        updatedByProperty.SetValue(entity, user);
                    }
                }
            }
        }
    }
}
