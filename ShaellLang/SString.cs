using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShaellLang;

public class SString : BaseValue
{
    private string _val;
    private NativeTable _nativeTable;
    
    public SString(string str)
        : base("string")
    {
        _val = str;
        _nativeTable = new NativeTable();
        
        _nativeTable.SetValue("length", new NativeFunc(LengthCallHandler, 0));
        _nativeTable.SetValue("substring", new NativeFunc(SubStringFunc, 2));
    }

    private IValue SubStringFunc(IEnumerable<IValue> argCollection)
    {
        Number[] args = argCollection.ToArray().Select(x => x.ToNumber()).ToArray();
        return new SString(Val.Substring((int) args[0].ToInteger(), (int) args[1].ToInteger()));
    }

    private IValue LengthCallHandler(IEnumerable<IValue> args) => new Number(this._val.Length);
    public override bool ToBool() => true;
    public override SString ToSString() => this;
    public override ITable ToTable() => _nativeTable;
    public override bool IsEqual(IValue other) => other is SString otherString && _val == otherString._val;
    public override string ToString() => _val;
    public static SString operator +(SString left, SString right) => new SString(left.Val + right.Val);
    public static SString operator *(SString left, Number right)
    {
        StringBuilder sb = new StringBuilder();
        if (right.IsInteger)
        {
            var val = right.ToInteger();
            if (val > int.MaxValue)
                throw new Exception("String multiplication overflow");
            if (val < 0)
                throw new Exception("String cannot be multiplied with less than 0");
            sb.Insert(0, left.Val, (int) val);
        }
        else
        {
            var floored = Math.Floor(right.ToFloating());
            if (floored > int.MaxValue)
                throw new Exception("String multiplication overflow");
            if (floored < 0)
                throw new Exception("String cannot be multiplied with less than 0");
            sb.Insert(0, left.Val, (int) Math.Floor(right.ToFloating()));
        }

        return new SString(sb.ToString());
    }

    public string Val => _val;
    public override int GetHashCode() => ("S" + Val).GetHashCode();
    public override bool Equals(object? obj) => obj is SString str && IsEqual(str);
}