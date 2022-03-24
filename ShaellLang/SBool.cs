using System;

namespace ShaellLang;

public class SBool : IValue
{
    private bool _value;
    public SBool(bool value)
    {
        _value = value;
    }


    public bool ToBool() => _value;
    public Number ToNumber() => _value ? new Number(1) : new Number(0);
    public IFunction ToFunction()
    {
        throw new Exception("Type error, bool cannot be converted to function");
    }

    public SString ToSString() => new SString(_value ? "true" : "false");
}