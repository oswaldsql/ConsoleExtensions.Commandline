namespace Demo
{
	using System;
	using System.Collections.Generic;

	public class ControllerSettings
	{
		internal Dictionary<Type, ControllerSetting> settings = new Dictionary<Type, ControllerSetting>();

		public ControllerSettings Set<T>(T setting)
			where T : ControllerSetting
		{
			if (setting == null)
			{
				if (this.settings.ContainsKey(typeof(T)))
				{
					this.settings.Remove(typeof(T));
				}
			}
			else
			{
				if (this.settings.ContainsKey(typeof(T)))
				{
					var mergeable = this.settings as IMergeable<T>;
					if (mergeable != null)
					{
						mergeable.Merge(setting);
					}

					return this;
				}

				this.settings[typeof(T)] = setting;
			}

			return this;
		}

		public bool TryGet<T>(out T setting)
			where T : ControllerSetting
		{
			this.settings.TryGetValue(typeof(T), out var baseSetting);

			setting = baseSetting as T;

			return setting != null;
		}

		public T Get<T>(Func<T> defaultValue = null)
			where T : ControllerSetting
		{
			if (this.settings.TryGetValue(typeof(T), out var baseSetting))
			{
				if (baseSetting is T setting)
				{
					return setting;
				}
			}

			var invoke = defaultValue?.Invoke();
			this.settings[typeof(T)] = invoke;

			return invoke;
		}
	}
}