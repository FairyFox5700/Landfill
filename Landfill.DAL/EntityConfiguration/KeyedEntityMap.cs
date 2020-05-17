using Landfill.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Landfill.DAL.Implementation.EntityConfiguration
{
    //https://stackoverflow.com/questions/53986595/one-to-one-relation-in-ef-core-using-fluent-api
    //    https://github.com/dotnet-architecture/eShopOnContainers/blob/43fe719e98bb7e004c697d5724a975f5ecb2191b/src/Services/Ordering/Ordering.Infrastructure/EntityConfigurations/OrderEntityTypeConfiguration.cs
    //    https://stackoverflow.com/questions/40154440/mapping-inheritance-in-entityframework-core
    //    https://stackoverflow.com/questions/46179902/how-to-create-table-per-type-inheritance-in-entity-framework-core-2-0-code-first
    public class KeyedEntityMap<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TKey>
    where TKey : struct
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
        }
    }
}
