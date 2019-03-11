using System;

namespace Demo
{
	using System.Linq;
	using System.Reflection;

	class Program
	{
		static void Main(string[] args)
		{
			Controller.Init<DemoObject>().WithArguments(args).Run();

			Console.ReadLine();
		}
	}

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
			FindAction(ref args);
			PopulateParameters(ref args);
			PopulateFlags(ref args);
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

		public void Run()
		{
			if (this.action != null)
			{
				Console.WriteLine(this.action.Invoke(this.source, this.actionArguments));
			}
		}
	}

	public class DemoObject
	{
		public string Name { get; set; }

		public string Say(string what)
		{
			return $"{what} {Name}";
		}
	}
}
