using System;
using System.Collections.Generic;

namespace ShaellLang;

public class SString : NativeTable, IValue, IKeyable
{
    private string _val;

    public SString(string str)
    {
        _val = str;
        
        this.SetValue("length", new NativeFunc(lengthCallHandler, 0));
    }

    private IValue lengthCallHandler(ICollection<IValue> args)
    {
        return new Number(this._val.Length);
    }

    public bool ToBool() => true;
    public Number ToNumber() => new Number(int.Parse(_val));

    public IFunction ToFunction() => throw new Exception("Cannot convert string to function");

    public SString ToSString() => this;
    public ITable ToTable() => this;
    
    public override RefValue GetValue(IKeyable key)
    {
        if (key is Number numberKey)
        {
            if (numberKey.IsInteger)
            {
                var val = numberKey.ToInteger();
                if (val >= 0 && val < _val.Length)
                {
                    //val is less than _val.Length which is an int, therefore val can safely be casted to int
                    return new RefValue(new SString(new string(_val[(int) val], 1)));
                }
            }
        }
        return base.GetValue(key);
    }

    public string Val => _val;
    public string KeyValue => _val;
    public string UniquePrefix => "S";
}