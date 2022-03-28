using System;
using System.Collections.Generic;
using System.Collections;

namespace ShaellLang 
{
	public delegate IValue NativeFuncCallHandler(ICollection<IValue> args);
	public class NativeFunc : IFunction 
	{
		private NativeFuncCallHandler _callHandler;

		public NativeFunc(NativeFuncCallHandler callHandler, uint argumentCount)
		{
			_callHandler = callHandler;
			ArgumentCount = argumentCount;
		}

		//Implement IFunction
		
		public IValue Call(ICollection<IValue> args) => _callHandler(args);

		public uint ArgumentCount {  get; private set; }
		
		public bool ToBool() => true;

		public Number ToNumber()
		{
			throw new Exception("Type error, function cannot be converted to number");
		}

		public IFunction ToFunction() => this;

		public SString ToSString()
		{
			throw new Exception("Type error, function cannot be converted to string");
		}

		public ITable ToTable()
		{
			throw new Exception("Type error, function cannot be converted to table");
		}
	}
}