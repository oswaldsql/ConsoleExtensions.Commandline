namespace Demo
{
	public interface IMergeable<T>
	{
		void Merge<T>(T newSettings);
	}
}