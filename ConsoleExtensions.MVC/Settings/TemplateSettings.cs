namespace Demo
{
	using ConsoleExtensions.Templating;

	public class TemplateSettings : ControllerSetting
	{
		public TemplateParser Parser { get; }

		public TemplateSettings() : this(TemplateParser.Default)
		{
			
		}

		public TemplateSettings(TemplateParser parser)
		{
			this.Parser = parser;
		}
	}
}