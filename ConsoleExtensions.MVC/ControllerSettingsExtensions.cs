namespace Demo
{
	using ConsoleExtensions.Proxy;

	public static class ControllerSettingsExtensions
	{
		public static ControllerSettings Proxy(this ControllerSettings settings, IConsoleProxy proxy)
		{
			return settings.Set(new ProxySetting(proxy));
		}

		public static ControllerSettings ViewFor<T>(this ControllerSettings settings, string template)
		{
			settings.Get(() => new TemplateSettings()).Parser.AddTypeTemplate<T>(template);

			return settings;
		}
	}
}