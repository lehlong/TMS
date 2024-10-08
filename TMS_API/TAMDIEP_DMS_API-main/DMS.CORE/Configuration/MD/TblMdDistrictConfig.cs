using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DMS.CORE.Entities.MD;

namespace DMS.CORE.Configuration.MD
{
    public class TblMdDistrictConfig : IEntityTypeConfiguration<TblMdDistrict>
    {
        public void Configure(EntityTypeBuilder<TblMdDistrict> builder)
        {
            builder.HasOne(x => x.Provine).WithMany(x=>x.Districts).HasForeignKey(x => x.ProvineId).IsRequired();
            builder.HasMany(x => x.Wards).WithOne(x=>x.District).HasForeignKey(x => x.DistrictId).IsRequired();
        }
    }
}
