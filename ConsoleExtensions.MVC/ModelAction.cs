namespace Demo
{
	using System;
	using System.Reflection;

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

		public ConsoleKeyInfo[] ShortcutKeys { get; set; }

		public string Description { get; set; }
	}
}