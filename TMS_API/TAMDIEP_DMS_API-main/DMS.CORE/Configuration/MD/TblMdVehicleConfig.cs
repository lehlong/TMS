using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DMS.CORE.Entities.MD;

namespace DMS.CORE.Configuration.MD
{
    public class TblMdVehicleConfig : IEntityTypeConfiguration<TblMdVehicle>
    {
        public void Configure(EntityTypeBuilder<TblMdVehicle> builder)
        {
            builder.HasOne(x => x.PartnerCreate).WithMany().HasForeignKey(x => x.PartnerIdCreate).IsRequired(false);
            builder.HasOne(x => x.Type).WithMany().HasForeignKey(x => x.VehicleTypeCode).IsRequired();
        }
    }
}
