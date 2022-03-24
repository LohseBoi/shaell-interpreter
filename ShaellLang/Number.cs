using System;
using System.Globalization;

namespace ShaellLang
{
    public class Number : IValue, IKeyable
    {
        private dynamic _numberRepresentation;

        public Number(long value)
        {
            _numberRepresentation = value;
        }

        public Number(double value)
        {
            _numberRepresentation = value;
        }

        public bool IsInteger => _numberRepresentation is long;

        public bool IsFloating => _numberRepresentation is double;

        public long ToInteger() => Convert.ToInt64(_numberRepresentation);
        public double ToFloating() => Convert.ToDouble(_numberRepresentation);
        public string KeyValue => Convert.ToString(_numberRepresentation);
        public string UniquePrefix => "N";

        public bool ToBool() => true;

        public Number ToNumber()
        {
            return this;
        }

        public IFunction ToFunction()
        {
            throw new Exception("Type error, number cannot be converted to function");
        }

        public SString ToSString()
        {
            if (IsFloating)
            {
                return new SString(ToFloating().ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                return new SString(ToInteger().ToString());
            }
        }
        public static Number operator +(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating)
            {
                return new Number(a.ToFloating() + b.ToFloating());
            }
            else
            {
                // Does not check for overflow where it should switch to floating
                return new Number(a.ToInteger() + b.ToInteger());
            }
        }
    }
}