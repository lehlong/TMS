using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DMS.CORE.Entities.SO;

namespace DMS.CORE.Configuration.SO
{
    public class TblOrderConfig : IEntityTypeConfiguration<TblSoOrder>
    {
        public void Configure(EntityTypeBuilder<TblSoOrder> builder)
        {
            builder.HasOne(x=>x.Parent).WithMany(x=>x.Childs).HasForeignKey(x=>x.ParentCode).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.PartnerBuy).WithMany().HasForeignKey(x => x.PartnerIdBuy).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.PartnerSell).WithMany().HasForeignKey(x => x.PartnerIdSell).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
