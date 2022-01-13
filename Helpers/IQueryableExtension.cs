using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Events.API.Helpers
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if(mappingDictionary == null)
            {
                throw new ArgumentNullException(nameof(mappingDictionary));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            string orderByString = string.Empty;

            // the orderBy string is separated by ",", so we split it.
            string[] orderByAfterSplit = orderBy.Split(",");

            foreach(string orderByClause in orderByAfterSplit)
            {
                string orderByClauseTrimmed = orderByClause.Trim();

                bool orderDesc = orderByClauseTrimmed.EndsWith(" desc");

                int spaceIndex = orderByClauseTrimmed.IndexOf(" ");
                string propertyName = spaceIndex == -1 ?
                    orderByClauseTrimmed : orderByClauseTrimmed.Remove(spaceIndex);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Couldn't find mapping for {propertyName}");
                }

                var propertyMappingValue = mappingDictionary[propertyName];

                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException($"Couldn't find propert mapping value for property {propertyName}");
                }

                foreach(var destProperty in propertyMappingValue.DestinationProperties)
                {
                    if (propertyMappingValue.ReverseOrder)
                    {
                        orderDesc = !orderDesc;
                    }

                    orderByString = orderByString + 
                        (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                        + destProperty
                        + (orderDesc ? " descending" : " ascending");
                }
            }

            return source.OrderBy(orderByString);
        }
    }
}
