using Landfill.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Landfill.DAL.Implementation.EntityConfiguration
{
    public class FaqEntityTypeConfiguration : KeyedEntityMap<FAQ, int>
    {
        public override void Configure(EntityTypeBuilder<FAQ> contentConfiguration)
        {
            contentConfiguration.ToTable("faqs");
            base.Configure(contentConfiguration);
            contentConfiguration.Property(fr=>fr.Tag).IsRequired();
            contentConfiguration.HasOne(fr=> fr.Content)
               .WithOne(c => c.Faq)
           .HasForeignKey<FAQ>(e => e.ContentId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
