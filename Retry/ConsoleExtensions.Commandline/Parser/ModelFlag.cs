namespace ConsoleExtensions.Commandline.Parser
{
    using System;
    using System.Reflection;

    public class ModelFlag
    {
        public ModelFlag(string name)
        {
            this.Name = name;
        }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public string Name { get; }

        public PropertyInfo Property { get; set; }

        public ConsoleKeyInfo[] ShortcutKeys { get; set; }

        public object Source { get; set; }

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

            this.Property.SetMethod.Invoke(this.Source, new[] { o });
        }
    }
}