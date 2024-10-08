using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DMS.CORE.Entities.MD;

namespace DMS.CORE.Configuration.MD
{
    public class TblMdPartnerConfig : IEntityTypeConfiguration<TblMdPartner>
    {
        public void Configure(EntityTypeBuilder<TblMdPartner> builder)
        {
            builder.HasOne(x => x.Provine).WithMany().HasForeignKey(x=>x.ProvineId).IsRequired(false);
            builder.HasOne(x => x.District).WithMany().HasForeignKey(x => x.DistrictId).IsRequired(false);
            builder.HasOne(x => x.Ward).WithMany().HasForeignKey(x => x.WardId).IsRequired(false);
            builder.HasIndex(x => x.Code).IsUnique();
            builder.HasOne(x => x.SaleType).WithMany(x => x.Partners).HasForeignKey(x => x.SaleTypeCode).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(x => x.AccountSaleOffices)
            .WithOne(g => g.Partner)
            .HasForeignKey(x => x.PartnerId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
