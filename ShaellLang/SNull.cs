using System;

namespace ShaellLang
{
    //Snull becuase Null is taken
    public class SNull : BaseValue
    {
        public SNull() : base("null") { }

        public override bool ToBool() => false;
        public override SString ToSString() => new("null");
        public override bool IsEqual(IValue other) => other is SNull;
    }
}