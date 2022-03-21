using System.Collections.Generic;
using System.Collections;

namespace ShaellLang 
{
	public delegate IValue NativeFuncCallHandler(ICollection<IValue> args);
	public class NativeFunc : IFunction 
	{
		private NativeFuncCallHandler _callHandler;

		public NativeFunc(NativeFuncCallHandler callHandler, uint argumnetCount)
		{
			_callHandler = callHandler;
			ArgumentCount = argumnetCount;
		}

		//Implement IFunction
		
		public IValue Call(ICollection<IValue> args) => _callHandler(args);

		public uint ArgumentCount {  get; private set; }
	}
}