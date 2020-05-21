using Landfill.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lanfill.BAL.Implementation.Mapping
{
    public class MappingContainer<TDto, TEntity> where TDto : class, new() where TEntity : BaseEntity<int>, new()
    {
        public List<MappedProperties<TDto, TEntity>> Mappings { get; set; }

        public MappingContainer(params MappedProperties<TDto, TEntity>[] maps)
        {
            this.Mappings = maps.ToList();
        }
        public PropertyInfo GetMappingFromMemberName<T>(string name)
        {
            if (typeof(T) == typeof(TDto))
            {
                return this.Mappings.SingleOrDefault(x => x.DtoProperty.Name == name).EntityProperty;
            }
            else if (typeof(T) == typeof(TEntity))
            {
                return this.Mappings.SingleOrDefault(x => x.EntityProperty.Name == name).DtoProperty;
            }
            throw new Exception($"Cannot get mapping for {typeof(T).Name} from MappedConverter<{typeof(TDto).Name}, {typeof(TEntity).Name}>");
        }

       
    }



}
