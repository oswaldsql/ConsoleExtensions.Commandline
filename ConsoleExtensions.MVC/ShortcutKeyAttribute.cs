namespace Demo
{
	using System;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class ShortcutKeyAttribute : Attribute
	{
		public ConsoleKeyInfo Key { get; }

		public ShortcutKeyAttribute(ConsoleKeyInfo key)
		{
			this.Key = key;
		}

		public ShortcutKeyAttribute(char c)
		{
			this.Key = new ConsoleKeyInfo(c, ConsoleKey.C, false,false,false);
		}
	}
}