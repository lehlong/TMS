using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DMS.CORE.Entities.MD;

namespace DMS.CORE.Configuration.MD
{
    public class TblMdItemConfig : IEntityTypeConfiguration<TblMdItem>
    {
        public void Configure(EntityTypeBuilder<TblMdItem> builder)
        {
            builder.HasOne(x => x.Type).WithMany().HasForeignKey(x => x.TypeCode).IsRequired(false);
            builder.HasOne(x => x.Group).WithMany().HasForeignKey(x => x.GroupCode).IsRequired(false);
        }
    }
}
