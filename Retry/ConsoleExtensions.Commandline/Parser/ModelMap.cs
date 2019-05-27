namespace ConsoleExtensions.Commandline.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ModelMap
    {
        public ModelMap(IEnumerable<ModelFlag> flags, IEnumerable<ModelAction> actions)
        {
            this.Flags = new ReadOnlyDictionary<string, ModelFlag>(
                flags.ToDictionary(s => s.Name, StringComparer.InvariantCultureIgnoreCase));
            this.Actions = new ReadOnlyDictionary<string, ModelAction>(
                actions.ToDictionary(s => s.Name, StringComparer.InvariantCultureIgnoreCase));
        }

        public IReadOnlyDictionary<string, ModelAction> Actions { get; }

        public IReadOnlyDictionary<string, ModelFlag> Flags { get; }

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
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public object Invoke(string action, params string[] parameters)
        {
            var method = this.Actions.Values.FirstOrDefault(a => a.Name == action)
                         ?? this.Actions.Values.FirstOrDefault(
                             a => a.Name.Equals(action, StringComparison.InvariantCultureIgnoreCase));

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

    public interface IValueConverter<T>
    {
        string ToString(T source);

        T ToValue(string source);
    }

    public class CustomValueConverter<T> : IValueConverter<T>
    {
        private readonly Func<T, string> toString;

        private readonly Func<string, T> toValue;

        public CustomValueConverter(Func<T, string> toString, Func<string, T> toValue)
        {
            this.toString = toString;
            this.toValue = toValue;
        }

        public string ToString(T source)
        {
            return this.toString(source);
        }

        public T ToValue(string source)
        {
            return this.toValue(source);
        }
    }
}