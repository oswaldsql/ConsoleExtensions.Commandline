namespace ConsoleExtensions.Commandline.Parser
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class ModelMapper
    {
        public static ModelMap Parse(object model)
        {
            return new ModelMap(PopulateFlags(model), PopulateActions(model));
        }

        private static IEnumerable<MethodInfo> GetNotOverloadedMethodInfos(IEnumerable<MethodInfo> runtimeMethods)
        {
            return runtimeMethods
                .Where(t => t.IsPublic && !t.IsSpecialName && !t.IsConstructor && t.DeclaringType != typeof(object))
                .ToLookup(info => info.Name).Where(t => t.Count() == 1).Select(t => t.First());
        }

        private static IEnumerable<ModelAction> PopulateActions(object model)
        {
            var runtimeMethods = model.GetType().GetRuntimeMethods();
            var methodInfos = GetNotOverloadedMethodInfos(runtimeMethods);
            foreach (var method in methodInfos)
            {
                var action = new ModelAction(method.Name)
                                 {
                                     Method = method,
                                     Source = model,
                                     DisplayName =
                                         method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                                         ?? SplitCamelCase(method.Name),
                                     Description = method.GetCustomAttribute<DescriptionAttribute>()?.Description
                                 };

                yield return action;
            }
        }

        private static IEnumerable<ModelFlag> PopulateFlags(object model)
        {
            var propertyInfos = model.GetType().GetProperties();

            foreach (var info in propertyInfos.Where(t => t.GetMethod.GetParameters().Length == 0))
            {
                var flag = new ModelFlag(info.Name)
                               {
                                   Property = info,
                                   Source = model,
                                   DisplayName =
                                       info.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                                       ?? SplitCamelCase(info.Name),
                                   Description = info.GetCustomAttribute<DescriptionAttribute>()?.Description
                               };

                yield return flag;
            }
        }

        private static string SplitCamelCase(string str)
        {
            var replace = Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2");

            var splitCamelCase = Regex.Replace(replace, @"(\p{Ll})(\P{Ll})", "$1 $2");

            return splitCamelCase;
        }
    }
}