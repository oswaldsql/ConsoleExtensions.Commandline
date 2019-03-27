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
		public Dictionary<string, ModelAction> Actions = new Dictionary<string, ModelAction>(StringComparer.InvariantCultureIgnoreCase);

		public Dictionary<string, ModelFlag> Flags = new Dictionary<string, ModelFlag>(StringComparer.InvariantCultureIgnoreCase);

		public ModelMap(object model)
		{
			this.PopulateFlags(model);
			this.PopulateActions(model);
		}

		public static string SplitCamelCase(string str)
		{
			var replace = Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2");

			var splitCamelCase = Regex.Replace(replace, @"(\p{Ll})(\P{Ll})", "$1 $2");

			return splitCamelCase;
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
						           ShortcutKeys = info.GetCustomAttributes<ShortcutKeyAttribute>().Select(t => t.Key).ToArray(),
								   Description = info.GetCustomAttribute<DescriptionAttribute>()?.Description
					           };

				this.Flags.Add(flag.Name, flag);
			}
		}

		private static IEnumerable<MethodInfo> GetNotOverloadedMethodInfos(IEnumerable<MethodInfo> runtimeMethods)
		{
			return runtimeMethods.Where(t => t.IsPublic && !t.IsSpecialName && !t.IsConstructor && t.DeclaringType != typeof(object)).ToLookup(info => info.Name).Where(t => t.Count() == 1).Select(t => t.First());
		}

		private void PopulateActions(object model)
		{
			var runtimeMethods = model.GetType().GetRuntimeMethods();
			var methodInfos = GetNotOverloadedMethodInfos(runtimeMethods);
			foreach (var method in methodInfos)
			{
				var action = new ModelAction(method.Name)
					             {
						             Method = method,
						             Source = model,
						             DisplayName = method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? SplitCamelCase(method.Name),
						             Description = method.GetCustomAttribute<DescriptionAttribute>()?.Description,
						             ShortcutKeys = method.GetCustomAttributes<ShortcutKeyAttribute>().Select(t => t.Key).ToArray()
					             };
				this.Actions.Add(action.Name, action);
			}
		}

		public string this[string property]
		{
			get
			{
				if (this.Flags.TryGetValue(property, out var p))
				{
					return p.CurrentValue();
				}
				throw new ArgumentException();
			}
			set
			{
				if (this.Flags.TryGetValue(property, out var p))
				{
					p.Set(value);
				}
			}
		}

		public object Invoke(string action, params string[] parameters)
		{
			var method = this.Actions.Values.FirstOrDefault(a => a.Name == action) ?? this.Actions.Values.FirstOrDefault(a => a.Name.Equals(action, StringComparison.InvariantCultureIgnoreCase));

			var infos = method.Method.GetParameters();
			var p = new List<object>();
			for (var index = 0; index < infos.Length; index++)
			{
				var info = infos[index];
				object o;
				if (info.ParameterType.IsEnum)
				{
					o = Enum.Parse(info.ParameterType, parameters[index], true);
				}
				else
				{
					o = Convert.ChangeType(parameters[index], info.ParameterType);
				}

				p.Add(o);
			}

			return method.Method.Invoke(method.Source, p.ToArray());
		}
	}
}