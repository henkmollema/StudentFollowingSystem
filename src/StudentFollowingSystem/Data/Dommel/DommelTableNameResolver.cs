using System;
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
                    // todo: exception, null, fallback resolver or type.Name?
                    throw new Exception(string.Format("Could not find the mapping for type '{0}'.", type.FullName));
                }

                return mapping.TableName;
            }

            return type.Name + "s";
        }
    }
}
