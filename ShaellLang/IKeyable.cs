namespace ShaellLang
{
	public interface IKeyable
	{
		string KeyValue { get; }
		string UniquePrefix { get; }
		public string ToString() => UniquePrefix + KeyValue;
	}
}