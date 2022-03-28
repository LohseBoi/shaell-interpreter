using System;
using System.Collections.Generic;

namespace ShaellLang
{
	public class UserTable : ITable, IValue
	{
		private Dictionary<string, RefValue> values;
		private string KeyValue(IKeyable key) => key.UniquePrefix + key.KeyValue;

		public UserTable()
		{
			values = new Dictionary<string, RefValue>();
		}
		
		private RefValue SetValue(IKeyable key, IValue value)
		{
			RefValue refValue = new RefValue(value);
			values.Add(KeyValue(key), refValue);
			return refValue;
		}
		
		public RefValue GetValue(IKeyable key)
		{
			bool exists = values.TryGetValue(KeyValue(key), out RefValue value);
			if (exists)
			{
				return value;
			}
			//Since keys should be implicitly defined we just add the key and return it
			return SetValue(key, new SNull());
			return null;
		}

		public void RemoveValue(IKeyable key)
		{
			values.Remove(KeyValue(key));
		}

		public bool ToBool()
		{
			return true;
		}

		public Number ToNumber()
		{
			throw new Exception("Type error: cannot convert table to number");
		}

		public IFunction ToFunction()
		{
			throw new Exception("Type error: cannot convert table to function");
		}

		public SString ToSString()
		{
			throw new Exception("Type error: cannot convert table to string");
		}

		public ITable ToTable()
		{
			return this;
		}
	}
}