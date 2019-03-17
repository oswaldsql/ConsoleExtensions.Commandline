namespace Demo
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using System.Text.RegularExpressions;

	public class ModelMap
	{
		public ModelMap(object model)
		{
			this.PopulateFlags(model);
			this.PopulateActions(model);
		}

		private void PopulateActions(object model)
		{
			var runtimeMethods = model.GetType().GetRuntimeMethods();
			var methodInfos = GetNotOverloadedMethodInfos(runtimeMethods);
			foreach (var method in methodInfos)
			{
				var action = new ModelAction(method.Name)
					             {
						             Source = model,
						             Method = method,
						             DisplayName = 	method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? SplitCamelCase(method.Name),
						             ShortcutKeys = method.GetCustomAttributes<ShortcutKeyAttribute>().Select(t => t.Key).ToArray()
					             };
				this.Actions.Add(action.Name, action);
			}
		}

		private static IEnumerable<MethodInfo> GetNotOverloadedMethodInfos(IEnumerable<MethodInfo> runtimeMethods)
		{
			return runtimeMethods.Where(t => t.IsPublic && !t.IsSpecialName && !t.IsConstructor && t.DeclaringType != typeof(object)).ToLookup(info => info.Name).Where(t => t.Count() == 1).Select(t => t.First());
		}

		public void PopulateFlags(object model)
		{
			var propertyInfos = model.GetType().GetProperties();

			foreach (var info in propertyInfos.Where(t => t.GetMethod.GetParameters().Length == 0))
			{
				var flag = new ModelFlag(info.Name)
					           {
						           Property = info,
						           Source = model,
						           DisplayName = info.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? SplitCamelCase(info.Name),
						           ShortcutKeys = info.GetCustomAttributes<ShortcutKeyAttribute>().Select(t => t.Key).ToArray()
					           };
				this.Flags.Add(flag.Name, flag);
			}
		}

		public Dictionary<string, ModelAction> Actions = new Dictionary<string, ModelAction>(StringComparer.InvariantCultureIgnoreCase);
		public Dictionary<string, ModelFlag> Flags = new Dictionary<string, ModelFlag>(StringComparer.InvariantCultureIgnoreCase);

		public static string SplitCamelCase(string str )
		{
			return Regex.Replace( 
				Regex.Replace( 
					str, 
					@"(\P{Ll})(\P{Ll}\p{Ll})", 
					"$1 $2" 
				), 
				@"(\p{Ll})(\P{Ll})", 
				"$1 $2" 
			);
		}
	}
}