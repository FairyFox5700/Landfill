using Landfill.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.DAL.Implementation.EntityConfiguration
{
    public class ContentTranslationTypeConfiguration : KeyedEntityMap<ContentTranslation, int>
    {
        public override void Configure(EntityTypeBuilder<ContentTranslation> contentConfiguration)
        {
            var converter = new EnumToStringConverter<Language>();
            contentConfiguration.ToTable("contentTranslations");
            base.Configure(contentConfiguration);
            contentConfiguration.Property(ctr => ctr.Language)
                .HasConversion(converter).IsRequired();
            contentConfiguration.Property(ctr => ctr.Text).IsRequired();
            contentConfiguration.HasOne<Content>(c => c.Content)
               .WithMany(c => c.Translations)
               .HasForeignKey(ctr=>ctr.ContentId)
                .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
