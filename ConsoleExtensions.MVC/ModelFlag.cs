namespace Demo
{
	using System;
	using System.Reflection;

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

		public ConsoleKeyInfo[] ShortcutKeys { get; set; }

		public string Description { get; set; }

		public string CurrentValue()
		{
			return this.Property.GetMethod.Invoke(this.Source, new object[0]).ToString();
		}

		public void Set(string s)
		{
			object o;
			if (this.Property.PropertyType.IsEnum)
			{
				o = Enum.Parse(this.Property.PropertyType, s, true);
			}
			else
			{
				o = Convert.ChangeType(s, this.Property.PropertyType);
			}

			this.Property.SetMethod.Invoke(this.Source, new []{o });
		}
	}
}