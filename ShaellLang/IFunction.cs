using System.Collections.Generic;
using System.Collections;

namespace ShaellLang 
{
	public interface IFunction : IValue
	{
		IValue Call(IEnumerable<IValue> args);
		
		uint ArgumentCount { get; }
	}
}
