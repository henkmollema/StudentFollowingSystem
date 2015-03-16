using System;
using System.Collections.Generic;
using System.Reflection;
using Dommel;

namespace StudentFollowingSystem.Data.Dommel
{
    public class CustomDommelPropertyResolver : DommelMapper.DefaultPropertyResolver
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
    }
}