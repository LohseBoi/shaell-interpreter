using System;

namespace ShaellLang
{
    //Snull becuase Null is taken
    public class SNull : IValue
    {
        public SNull()
        {
            
        }

        public bool ToBool() => false;
        public Number ToNumber()
        {
            throw new Exception("Cannot implicitly null to number");
        }

        public IFunction ToFunction()
        {
            throw new Exception("Type error, null cannot be converted to function");
        }

        public SString ToSString() => new SString("null");
    }
}