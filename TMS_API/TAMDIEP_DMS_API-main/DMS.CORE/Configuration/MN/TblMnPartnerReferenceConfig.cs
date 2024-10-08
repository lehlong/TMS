using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DMS.CORE.Entities.MN;

namespace DMS.CORE.Configuration.MN
{
    public class TblMnPartnerReferenceConfig : IEntityTypeConfiguration<TblMnPartnerReference>
    {
        public void Configure(EntityTypeBuilder<TblMnPartnerReference> builder)
        {
            builder.HasIndex(x => new { x.PartnerIdBuy, x.PartnerIdSell }).IsUnique();
            builder.HasOne(x => x.PartnerBuy).WithMany(x => x.ReferenceBuys).HasForeignKey(x => x.PartnerIdBuy).IsRequired();
            builder.HasOne(x => x.PartnerSell).WithMany(x => x.ReferenceSells).HasForeignKey(x => x.PartnerIdSell).IsRequired();
        }
    }
}
