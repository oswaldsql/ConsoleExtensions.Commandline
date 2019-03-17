namespace Demo
{
	using System;
	using System.Linq;
	using System.Reflection;

	public class Controller
	{
		private readonly object source;

		private MethodInfo action;

		private object[] actionArguments;

		private Controller(object source)
		{
			this.source = source;
		}

		public static Controller Init<T>()
			where T : new()
		{
			return new Controller(new T());
		}

		public void Run()
		{
			if (this.action != null)
			{
				Console.WriteLine(this.action.Invoke(this.source, this.actionArguments));
			}
		}

		public Controller WithArguments(string[] args)
		{
			this.FindAction(ref args);
			this.PopulateParameters(ref args);
			this.PopulateFlags(ref args);
			return this;
		}

		private static bool FindMatchingMethod(MethodInfo info, string name)
		{
			return info.IsPublic && (info.Name.Equals(name) || info.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		private void FindAction(ref string[] args)
		{
			var name = args.FirstOrDefault();
			if (name == null || name.StartsWith("-"))
			{
				return;
			}

			var runtimeMethods = this.source.GetType().GetRuntimeMethods();
			var bestMatchMethod = runtimeMethods.FirstOrDefault(info => FindMatchingMethod(info, name));

			if (bestMatchMethod == null)
			{
				return;
			}

			this.action = bestMatchMethod;
			args[0] = null;
		}

		private int FindIndexOfFlag(string[] args)
		{
			for (var i = 0; i < args.Length; i++)
			{
				if (args[i] != null && args[i].StartsWith("-")) return i;
			}

			return -1;
		}

		private void PopulateFlags(ref string[] args)
		{
			var propertyInfos = this.source.GetType().GetProperties();

			var indexOfFlag = this.FindIndexOfFlag(args);

			while (indexOfFlag > -1)
			{
				var name = args[indexOfFlag].Substring(1);
				var stringValue = args[indexOfFlag + 1];

				args[indexOfFlag] = null;
				args[indexOfFlag + 1] = null;

				var propertyInfo = propertyInfos.FirstOrDefault(p => p.Name.Equals(name) || p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
				if (propertyInfo != null && propertyInfo.CanWrite)
				{
					propertyInfo.SetMethod.Invoke(this.source, new[]
						                                           {
							                                           Convert.ChangeType(stringValue, propertyInfo.PropertyType)
						                                           });
				}

				indexOfFlag = this.FindIndexOfFlag(args);
			}
		}

		private void PopulateParameters(ref string[] args)
		{
			var parameterInfos = this.action.GetParameters();

			this.actionArguments = new object[parameterInfos.Length];
			for (var i = 0; i < parameterInfos.Length; i++)
			{
				this.actionArguments[i] = Convert.ChangeType(args[i + 1], parameterInfos[i].ParameterType);
				args[i + 1] = null;
			}
		}
	}
}