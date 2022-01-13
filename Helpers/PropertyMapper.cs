using Events.API.Entities;
using Events.API.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Events.API.Helpers
{
    public class PropertyMapper : IPropertyMapper
    {
        private Dictionary<string, PropertyMappingValue> eventPropertMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id", new PropertyMappingValue(new List<string>(){ "Id" }) },
            {"Title", new PropertyMappingValue(new List<string>(){ "Title" }) },
            {"Description", new PropertyMappingValue(new List<string>(){ "Description" }) },
            {"EventType", new PropertyMappingValue(new List<string>(){ "EventType" }) },
            {"StartDateTime", new PropertyMappingValue(new List<string>(){ "StartDateTime" }) },
            {"EndDateTime", new PropertyMappingValue(new List<string>(){ "EndDateTime" }) },
        };

        private Dictionary<string, PropertyMappingValue> guestPropertMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Email", new PropertyMappingValue(new List<string>(){ "Email" }) },
            {"Name", new PropertyMappingValue(new List<string>(){ "FirstName", "LastName" }) },
            {"Age", new PropertyMappingValue(new List<string>(){ "DateOfBirth" }, true) }
        };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMapper()
        {
            propertyMappings.Add(new PropertyMapping<EventModel, Event>(eventPropertMapping));
            propertyMappings.Add(new PropertyMapping<GuestModel, Person>(guestPropertMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingProperty = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingProperty.Count() == 1)
            {
                return matchingProperty.First().MappingDictionary;
            }

            throw new Exception($"Can't find property mapping for <{typeof(TSource)}, {typeof(TDestination)}>");
        }

        public bool ValidatePropertyMapping<TSource, TDestination>(string orderByText)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(orderByText))
            {
                return true;
            }

            var orderByTextAfterSplit = orderByText.Split(',');

            foreach (var orderByClause in orderByTextAfterSplit)
            {
                var orderByClauseTrimmed = orderByClause.Trim();

                var spaceIndex = orderByClauseTrimmed.IndexOf(" ");
                var propertyName = spaceIndex == -1 ?
                    orderByClauseTrimmed : orderByClauseTrimmed.Remove(spaceIndex);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
