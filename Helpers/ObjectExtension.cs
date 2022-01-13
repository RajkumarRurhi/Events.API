using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Events.API.Helpers
{
    public static class ObjectExtension
    {
        public static ExpandoObject ShapeData<T>(this T source, string fields)
        {
            if(source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObject = new ExpandoObject();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var lstPropertyInfo = typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                foreach (var propertyInfo in lstPropertyInfo)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandoObject).Add(propertyInfo.Name, propertyValue);
                }

                return expandoObject;
            }

            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    throw new Exception($"Property {propertyName} wasn't found on {typeof(T)}");
                }

                var propertyValue = propertyInfo.GetValue(source);

                ((IDictionary<string, object>)expandoObject).Add(propertyInfo.Name, propertyValue);
            }

            return expandoObject;
        }
    }
}
