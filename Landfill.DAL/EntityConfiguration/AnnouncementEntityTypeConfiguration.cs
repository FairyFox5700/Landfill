using Landfill.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Landfill.DAL.Implementation.EntityConfiguration
{
    public class AnnouncementEntityTypeConfiguration : KeyedEntityMap<Announcement, int>
    {
        public override  void Configure(EntityTypeBuilder<Announcement> contentConfiguration)
        {
            contentConfiguration.ToTable("announcements");
            base.Configure(contentConfiguration);
            contentConfiguration.Property(ac => ac.Header).IsRequired();
            contentConfiguration.Property(ac => ac.ValiUntil).IsRequired();
            contentConfiguration.HasOne(ac => ac.Content)
               .WithOne(c=>c.Announcement)
           .HasForeignKey<Announcement>(e => e.ContentId)
            .OnDelete(DeleteBehavior.Restrict);
          
        }
    }
}
