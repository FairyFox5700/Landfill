using Landfill.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.DAL.Implementation.EntityConfiguration
{
    public class ContentEntityTypeConfiguration : KeyedEntityMap<Content, int>
    {
        public override void Configure(EntityTypeBuilder<Content> contentConfiguration)
        {
            var converter = new EnumToStringConverter<ContentType>();
            var converterForState = new EnumToStringConverter<State>();
            base.Configure(contentConfiguration);
            contentConfiguration.ToTable("contents");//, LandfillContext.DEFAULT_SCHEMA
            contentConfiguration.Property(c => c.State)
                .HasConversion(converterForState);
            contentConfiguration.Property(c => c.ContentType)
                .HasConversion(converter);
           
        }
    }
}
