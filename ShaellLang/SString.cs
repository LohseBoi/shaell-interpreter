using System;

namespace ShaellLang;

public class SString : IValue, IKeyable
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
    public ITable ToTable()
    {
        //Should return a table which can give length and so on for a string
        throw new Exception("Cannot convert string to table");
    }

    public string Val => _val;
    public string KeyValue => _val;
    public string UniquePrefix => "S";
}