using System;
using System.Text;

namespace ShaellLang;

public class BString : NativeTable, IValue
{
    public string Val { get; }
    public byte[] Buffer { get; } //TODO: In '_functionLookUp'??

    public BString(byte[] buf)
    {
        Buffer = buf;
        Val = Encoding.Default.GetString(buf);
    }
    public BString(string val) : this(Encoding.Default.GetBytes(val)) { }
    public bool ToBool() => true;

    public Number ToNumber() => new(int.Parse(Val));

    public IFunction ToFunction() => throw new Exception("Cannot convert string to function");

    public SString ToSString() => new(Val);

    public ITable ToTable() => this;
}