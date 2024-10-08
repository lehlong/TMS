using DMS.CORE.Entities.MN;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.CORE.Configuration.MN
{
    public class TblMnAccountSaleOfficeConfig : IEntityTypeConfiguration<TblMnAccountSaleOffice>
    {
        public void Configure(EntityTypeBuilder<TblMnAccountSaleOffice> builder)
        {
            builder.HasKey(x => new { x.UserName, x.PartnerId });
            builder.HasOne(x => x.Account).WithMany(x => x.AccountSaleOffices).HasForeignKey(x => x.UserName).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Partner).WithMany(x => x.AccountSaleOffices).HasForeignKey(x => x.PartnerId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
