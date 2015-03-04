using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper.FluentMap.Dommel.Mapping;
using Dapper.FluentMap.Mapping;
using Dommel;

namespace Dapper.FluentMap.Dommel.Resolvers
{
    /// <remarks>
    /// Quick-fix for bug in DommelTableNameResolver.
    /// https://github.com/henkmollema/Dapper-FluentMap/issues/26
    /// </remarks>
    public class DommelTableNameResolver2 : DommelMapper.ITableNameResolver
    {
        public string ResolveTableName(Type type)
        {
            IEntityMap entityMap;
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                var mapping = entityMap as IDommelEntityMap;

                if (mapping == null)
                {
                    throw new Exception(string.Format("Could not find the mapping for type '{0}'.", type.FullName));
                }

                return mapping.TableName;
            }

            return type.Name + "s";
        }
    }

    public class DommelPropertyResolver2 : DommelMapper.PropertyResolverBase
    {
        protected override IEnumerable<PropertyInfo> FilterComplexTypes(IEnumerable<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
            {
                Type type = propertyInfo.PropertyType;
                type = Nullable.GetUnderlyingType(type) ?? type;
                if (type.IsPrimitive || type.IsEnum || PrimitiveTypes.Contains(type))
                {
                    yield return propertyInfo;
                }
            }
        }

        public override IEnumerable<PropertyInfo> ResolveProperties(Type type)
        {
            IEntityMap entityMap;
            if (FluentMapper.EntityMaps.TryGetValue(type, out entityMap))
            {
                foreach (var property in FilterComplexTypes(type.GetProperties()))
                {
                    var propertyMap = entityMap.PropertyMaps.FirstOrDefault(p => p.PropertyInfo.Name == property.Name);
                    if (propertyMap != null)
                    {
                        yield return !propertyMap.Ignored ? property : null;
                    }
                    else
                    {
                        yield return property;
                    }
                }
            }
            else
            {
                foreach (var property in FilterComplexTypes(type.GetProperties()))
                {
                    yield return property;
                }
            }
        }
    }
}
