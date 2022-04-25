using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ShaellLang
{
    public class Number : BaseValue
    {
        private dynamic _numberRepresentation;

        private NumberTable _numberTable
        {
            get => NumberTable.GetInstance(this);
        }

        public Number(long value)
            : base("integernumber")
        {
            _numberRepresentation = value;
        }

        public Number(double value)
            : base("floatingnumber")
        {
            _numberRepresentation = value;
        }

        public bool IsInteger => _numberRepresentation is long;

        public bool IsFloating => _numberRepresentation is double;

        public long ToInteger() => Convert.ToInt64(_numberRepresentation);
        
        public double ToFloating() => Convert.ToDouble(_numberRepresentation);
        public string KeyValue => Convert.ToString(_numberRepresentation);
        public string UniquePrefix => "N";

        public override bool ToBool() => true;

        public override Number ToNumber() => this;

        public override SString ToSString()
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

        public override ITable ToTable() => _numberTable;

        public override int GetHashCode() => ("N" + ToSString().Val).GetHashCode();

        public override bool Equals(object? obj) => obj is Number number && IsEqual(number);
        
        public static Number operator +(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating)
                return new Number(a.ToFloating() + b.ToFloating());
            else
            {
                try
                {
                    return new Number(checked(a.ToInteger() + b.ToInteger()));
                }
                catch (OverflowException)
                {
                    return new Number(a.ToFloating() + b.ToFloating());
                }
            }
        }
        
        public static Number operator -(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating)
                return new Number(a.ToFloating() - b.ToFloating());
            else
            {
                try
                {
                    return new Number(checked(a.ToInteger() - b.ToInteger()));
                }
                catch (OverflowException)
                {
                    return new Number(a.ToFloating() - b.ToFloating());
                }
            }
        }

        public static Number operator *(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating)
                return new Number(a.ToFloating() * b.ToFloating());
            else
            {
                try
                {
                    return new Number(checked(a.ToInteger() * b.ToInteger()));
                }
                catch (OverflowException)
                {
                    return new Number(a.ToFloating() * b.ToFloating());
                }
            }
        }

        public static Number operator /(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating || a.ToInteger() % b.ToInteger() != new Number(0).ToInteger())
                return new Number(a.ToFloating() / b.ToFloating());
            else
                return new Number(a.ToInteger() / b.ToInteger());
        }
        
        public static Number operator %(Number a, Number b)
        {
            if (a.IsFloating || b.IsFloating)
                return new Number(a.ToFloating() % b.ToFloating());
            return new Number(a.ToInteger() % b.ToInteger());
        }

        public static Number Power(Number a, Number b) => new Number(Math.Pow(a.ToFloating(), b.ToFloating()));
        
        //overide unary - and return the negative of the number
        public static Number operator -(Number a)
        {
            if (a.IsFloating)
                return new Number(-a.ToFloating());
            return new Number(-a.ToInteger());
        }
        
        //Implement less than operator and convert to floating and integer comparison correctly
        public static bool operator <(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() < b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() < b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() < b.ToFloating();
            return a.ToInteger() < b.ToInteger();
        }
        
        //Implement greater than operator and convert to floating and integer comparison correctly
        public static bool operator >(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() > b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() > b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() > b.ToFloating();
            return a.ToInteger() > b.ToInteger();
        }

        public static bool operator <=(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() <= b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() <= b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() <= b.ToFloating();
            return a.ToInteger() <= b.ToInteger();
        }
        
        public static bool operator >=(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() >= b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() >= b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() >= b.ToFloating();
            return a.ToInteger() >= b.ToInteger();
        }

        public override bool IsEqual(IValue other) => other is Number otherNumber && this == otherNumber;
        
        public static bool operator ==(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() == b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() == b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() == b.ToFloating();
            return a.ToInteger() == b.ToInteger();
        }

        public static bool operator !=(Number a, Number b)
        {
            if (a.IsFloating && b.IsFloating)
                return a.ToFloating() != b.ToFloating();
            if (a.IsFloating && b.IsInteger)
                return a.ToFloating() != b.ToInteger();
            if (a.IsInteger && b.IsFloating)
                return a.ToInteger() != b.ToFloating();
            return a.ToInteger() != b.ToInteger();
        }

        public static bool operator !(Number a) => !a.ToBool();
    }
}