using DMS.CORE.Entities.BU;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DMS.CORE.Configuration.CM
{
    public class TblCommentConfig : IEntityTypeConfiguration<TblCmComment>
    {
        public void Configure(EntityTypeBuilder<TblCmComment> builder)
        {
            builder.HasMany(x => x.Replies)
                .WithOne(x => x.PComment)
                .HasForeignKey(x => x.PId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
