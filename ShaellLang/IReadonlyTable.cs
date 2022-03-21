namespace ShaellLang
{
	public interface IReadonlyTable
	{
		IValue GetValue(IKeyable key);
		void RemoveValue(IKeyable key);
	}
}