namespace ShaellLang
{
	public interface ITable
	{
		//Gets a key
		RefValue GetValue(IValue key);
		//Removes a key
		void RemoveValue(IValue key);
	}
}