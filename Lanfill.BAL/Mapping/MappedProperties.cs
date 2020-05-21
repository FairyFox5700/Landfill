using System.Reflection;

namespace Lanfill.BAL.Implementation.Mapping
{
    public class MappedProperties<TDto, TEntity>
    {
        public PropertyInfo DtoProperty { get; }
        public PropertyInfo EntityProperty { get; }
    }



}
