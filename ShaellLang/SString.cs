using System;

namespace ShaellLang;

public class SString : IValue
{
    private string _val;

    public SString(string str)
    {
        _val = str;
    }

    public bool ToBool() => true;
    public Number ToNumber() => new Number(int.Parse(_val));

    public IFunction ToFunction() => throw new Exception("Cannot convert string to function");

    public SString ToSString() => this;

    public string Val => _val;
}