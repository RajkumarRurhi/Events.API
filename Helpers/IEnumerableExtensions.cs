using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Events.API.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<T>(this IEnumerable<T> source, string fields)
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var lstExpandoObject = new  List<ExpandoObject>();

            var lstPropertInfo = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var tmpLstPropertyInfo = typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                lstPropertInfo.AddRange(tmpLstPropertyInfo);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(T)}");
                    }

                    lstPropertInfo.Add(propertyInfo);
                }
            }

            foreach (T srcObject in source)
            {
                var shapedCstmObject = new ExpandoObject();

                foreach (var propertyInfo in lstPropertInfo)
                {
                    var propertyValue = propertyInfo.GetValue(srcObject);

                    ((IDictionary<string, object>)shapedCstmObject).Add(propertyInfo.Name, propertyValue);
                }

                // add the ExpandoObject to the list
                lstExpandoObject.Add(shapedCstmObject);
            }

            // return the list
            return lstExpandoObject;
        }
    }
}
