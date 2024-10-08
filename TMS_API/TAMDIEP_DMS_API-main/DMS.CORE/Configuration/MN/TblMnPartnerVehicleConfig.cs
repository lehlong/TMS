using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DMS.CORE.Entities.MN;

namespace DMS.CORE.Configuration.MN
{
    public class TblMnPartnerVehicleConfig : IEntityTypeConfiguration<TblMnPartnerVehicle>
    {
        public void Configure(EntityTypeBuilder<TblMnPartnerVehicle> builder)
        {
            builder.HasIndex(x => new { x.PartnerId, x.VehicleCode }).IsUnique();
            builder.HasOne(x => x.Vehicle).WithMany(x => x.PartnerReferences).HasForeignKey(x => x.VehicleCode).IsRequired();
            builder.HasOne(x => x.Partner).WithMany(x => x.VehicleReferences).HasForeignKey(x => x.PartnerId).IsRequired();

        }
    }
}
