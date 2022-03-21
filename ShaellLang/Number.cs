using System;

namespace ShaellLang
{
    public class Number : IValue
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

        public bool IsInteger
        {
            get => _numberRepresentation is long;
        }

        public bool IsFloating
        {
            get => _numberRepresentation is double;
        }

        public long ToInteger() => Convert.ToInt64(_numberRepresentation);
        public long ToFloating() => Convert.ToDouble(_numberRepresentation);
    }
}