using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleExtensions.Commandline.Converters
{
    public static class CustomAttributeProviderExtensions
    {
        public static bool TryGetCustomAttribute<T>(this ICustomAttributeProvider element, out T value)
            where T : Attribute
        {
            var attributes = element.GetCustomAttributes(typeof(T), true);

            if (attributes.Length > 0)
            {
                value = attributes.First() as T;
                return true;
            }

            value = null;
            return false;
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider element)
            where T : Attribute
        {
            var attributes = element.GetCustomAttributes(typeof(T), true);

            foreach (var attribute in attributes)
            {
                yield return (attribute as T);
            }
        }

    }
}