using System;

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

        public bool IsInteger
        {
            get => _numberRepresentation is long;
        }

        public bool IsFloating
        {
            get => _numberRepresentation is double;
        }

        public long ToInteger() => Convert.ToInt64(_numberRepresentation);
        public double ToFloating() => Convert.ToDouble(_numberRepresentation);
        public string KeyValue { get => Convert.ToString(_numberRepresentation); }
        public string UniquePrefix { get => "N"; }
    }
}