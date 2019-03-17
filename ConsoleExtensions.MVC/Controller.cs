namespace Demo
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using System.Text.RegularExpressions;

	using ConsoleExtensions.Proxy;
	using ConsoleExtensions.Templating;

	public class Controller
	{
		private readonly object source;

		private MethodInfo action;

		private object[] actionArguments;

		private Controller(object source)
		{
			this.source = source;
		}

		public static Controller Init<T>() where T:new()
		{
			return new Controller(new T());
		}

		public Controller WithArguments(string[] args)
		{
			this.FindAction(ref args);
			this.PopulateParameters(ref args);
			this.PopulateFlags(ref args);
			return this;
		}

		private void PopulateFlags(ref string[] args)
		{
			var propertyInfos = this.source.GetType().GetProperties();

			var indexOfFlag = this.FindIndexOfFlag(args);

			while (indexOfFlag > -1)
			{
				var name = args[indexOfFlag].Substring(1);
				var stringValue = args[indexOfFlag+1];

				args[indexOfFlag] = null;
				args[indexOfFlag+1] = null;

				var propertyInfo = propertyInfos.FirstOrDefault(p => p.Name.Equals(name) || p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
				if (propertyInfo != null && propertyInfo.CanWrite)
				{
					propertyInfo.SetMethod.Invoke(this.source, new[]
						                                           {
							                                           Convert.ChangeType(stringValue, propertyInfo.PropertyType)
						                                           });}

				indexOfFlag = this.FindIndexOfFlag(args);
			}
		}

		private int FindIndexOfFlag(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i]!= null && args[i].StartsWith("-")) return i;
			}

			return -1;
		}

		private void PopulateParameters(ref string[] args)
		{
			var parameterInfos = this.action.GetParameters();

			this.actionArguments = new object[parameterInfos.Length];
			for (int i = 0; i < parameterInfos.Length; i++)
			{
				this.actionArguments[i] = Convert.ChangeType(args[i + 1], parameterInfos[i].ParameterType);
				args[i + 1] = null;
			}
		}

		private void FindAction(ref string[] args)
		{
			var firstOrDefault = args.FirstOrDefault();
			if(firstOrDefault == null || firstOrDefault.StartsWith("-")) return;

			var runtimeMethods = this.source.GetType().GetRuntimeMethods();
			var bestMatchMethod = runtimeMethods.FirstOrDefault(info => info.IsPublic && (info.Name.Equals(firstOrDefault) || info.Name.Equals(firstOrDefault, StringComparison.InvariantCultureIgnoreCase)));

			if(bestMatchMethod == null) return;

			args[0] = null;

			this.action = bestMatchMethod;
		}

		private void Run(ControllerSettings settings)
		{
			if (this.Proxy == null)
			{
				this.Proxy = settings.Get<ProxySetting>().Proxy;
			}

			if (this.action != null)
			{
				object result;
				try
				{
					result = this.action.Invoke(this.source, this.actionArguments);
				}
				catch (TargetInvocationException e)
				{
					result = e.InnerException ?? e;
				}
				catch (Exception e)
				{
					result = e;
				}

				var template = settings.Get(() => new TemplateSettings()).Parser.Parse("{}");

				this.Proxy.WriteTemplate(template, result);
			}
		}

		public IConsoleProxy Proxy { get; set; }

		public static void Run(object model, string[] arguments, Action<ControllerSettings> setup = null)
		{
			var controllerSettings = new ControllerSettings();
			DefaultSettings(controllerSettings);
			setup?.Invoke(controllerSettings);

			var controller = new Controller(model);
			controller.WithArguments(arguments).Run(controllerSettings);
		}

		private static void DefaultSettings(ControllerSettings setup)
		{
			setup.Proxy(ConsoleProxy.Instance());
		}

		public static void Run<T>(string[] arguments, Action<ControllerSettings> setup = null) where T : new()
		{
			Run(new T(), arguments, setup);
		}
	}

	public class ModelMap
	{
		public ModelMap(object model)
		{
			PopulateFlags(model);
			PopulateActions(model);
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
						           DisplayName = info.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? SplitCamelCase(info.Name)
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

	public class ModelFlag
	{
		public ModelFlag(string name)
		{
			this.Name = name;
		}

		public string Name { get; private set; }

		public object Source { get; set; }

		public string DisplayName { get; set; }

		public PropertyInfo Property { get; set; }

		public string CurrentValue()
		{
			return Property.GetMethod.Invoke(Source, new object[0]).ToString();
		}

		public void Set(string s)
		{
			Property.SetMethod.Invoke(Source, new []{Convert.ChangeType(s, Property.PropertyType) });
		}
	}

	public class ModelAction
	{
		public MethodInfo Method { get; set; }

		public ModelAction(string name)
		{
			this.Name = name;
		}

		public string Name { get; private set; }

		public object Source { get; set; }

		public string DisplayName { get; set; }
	}
}