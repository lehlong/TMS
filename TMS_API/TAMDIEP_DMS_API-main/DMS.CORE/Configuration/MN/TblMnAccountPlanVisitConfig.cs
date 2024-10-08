using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DMS.CORE.Entities.MN;

namespace DMS.CORE.Configuration.MN
{
    public class TblMnAccountPlanVisitConfig : IEntityTypeConfiguration<TblMnAccountPlanVisit>
    {
        public void Configure(EntityTypeBuilder<TblMnAccountPlanVisit> builder)
        {
            builder.HasMany(x => x.AccountPlanVisitStores)
                .WithOne(g => g.AccountPlanVisit)
                .HasForeignKey(x => x.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.AccountCareStores)
                .WithOne(g => g.AccountPlanVisit)
                .HasForeignKey(x => x.PlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
