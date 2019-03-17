namespace Demo
{
	using ConsoleExtensions.Proxy;

	public class ProxySetting : ControllerSetting
	{
		public IConsoleProxy Proxy { get; }

		public ProxySetting(IConsoleProxy proxy)
		{
			this.Proxy = proxy;
		}
	}
}