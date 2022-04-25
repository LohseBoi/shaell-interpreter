using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShaellLang;

public class BString : BaseValue
{
    private string _val;
    private byte[] _buffer;
    private NativeTable _table;

    public BString(byte[] buf)
        : base("bstring")
    {
        _buffer = buf;
        _val = Encoding.Default.GetString(buf);
        _table = new NativeTable();
        _table.SetValue("toSString", new NativeFunc(ToSStringFunc, 1));
        _table.SetValue("substring", new NativeFunc(SubStringFunc, 2));
    }

    private IValue ToSStringFunc(IEnumerable<IValue> argCollection)
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
    }

    private IValue SubStringFunc(IEnumerable<IValue> argCollection)
    {
        Number[] args = argCollection.ToArray().Select(x => x.ToNumber()).ToArray();
        return new SString(_val.Substring((int) args[0].ToInteger(), (int) args[1].ToInteger()));
    }

    public BString(string val) : this(Encoding.Default.GetBytes(val)) { }
    public override bool ToBool() => true;

    public override Number ToNumber() => new(int.Parse(_val));

    public override SString ToSString() => new(_val);

    public override ITable ToTable() => _table;
    public override bool IsEqual(IValue other)
    {
        if (other is BString unpackedBString)
            return unpackedBString._buffer.SequenceEqual(_buffer);
        return false;
    }

    public string Val => _val;
    public byte[] Buffer => _buffer;
}