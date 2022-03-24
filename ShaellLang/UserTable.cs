using System.Collections.Generic;

namespace ShaellLang
{
	public class UserTable : IReadonlyTable 
	{
		private Dictionary<string, IValue> values;
		private string KeyValue(IKeyable key) => key.UniquePrefix + key.KeyValue;

		public void SetValue(IKeyable key, IValue value) 
		{
			values.Add(KeyValue(key), value);
		}
		
		public IValue GetValue(IKeyable key)
		{
			bool exists = values.TryGetValue(KeyValue(key), out IValue value);
			if (exists)
			{
				return value;
			}
			return new SNull();
		}

		public void RemoveValue(IKeyable key)
		{
			values.Remove(KeyValue(key));
		}
	}
}