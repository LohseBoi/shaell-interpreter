using System;
using System.Linq;
using System.Text;

namespace ShaellLang;

public class BString : NativeTable, IValue
{
    private string _val;
    private byte[] _buffer;

    public BString(byte[] buf)
    {
        _buffer = buf;
        _val = Encoding.Default.GetString(buf);
        SetValue("toSString", new NativeFunc(argCollection =>
        {
            Encoding e;
            switch (argCollection.ToArray()[0].ToSString().Val.ToLower())
            {
                case "ascii":
                    e = new ASCIIEncoding();
                    break;
                case "utf-8":
                    e = new UTF8Encoding();
                    break;
                case "utf-16":
                    e = new UnicodeEncoding();
                    break;
                case "utf-32":
                    e = new UTF32Encoding();
                    break;
                default:
                    throw new Exception("Wrong encoding");
            }
            return new SString(e.GetString(_buffer));
        }, 1));
        SetValue("substring", new NativeFunc(argCollection =>
        {
            Number[] args = argCollection.ToArray().Select(x => x.ToNumber()).ToArray();
            return new SString(_val.Substring((int)args[0].ToInteger(), (int)args[1].ToInteger()));
        }, 2));
    }
    public BString(string val) : this(Encoding.Default.GetBytes(val)) { }
    public bool ToBool() => true;

    public Number ToNumber() => new(int.Parse(_val));

    public IFunction ToFunction() => throw new Exception("Cannot convert string to function");

    public SString ToSString() => new(_val);

    public ITable ToTable() => this;
    
    public override RefValue GetValue(IKeyable key)
    {
        if (key is Number numberKey)
        {
            if (numberKey.IsInteger)
            {
                long val = numberKey.ToInteger();
                if (val >= 0 && val < _buffer.Length)
                {
                    //val is less than _val.Length which is an int, therefore val can safely be casted to int
                    return new RefValue(new Number(_buffer[val]));
                }
            }
        }
        return base.GetValue(key);
    }
    public string Val => _val;
    public byte[] Buffer => _buffer;
    public string KeyValue => _val;
    public string UniquePrefix => "S";
    
}