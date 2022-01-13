using System.Collections.Generic;

namespace Events.API.Helpers
{
    public interface IPropertyMapper
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidatePropertyMapping<TSource, TDestination>(string orderByText);
    }
}