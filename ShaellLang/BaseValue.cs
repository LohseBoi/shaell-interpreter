using System;

namespace ShaellLang;

public abstract class BaseValue : IValue
{
    private readonly string _typeName;

    protected BaseValue(string typeName)
    {
        _typeName = typeName;
    }
    public virtual bool ToBool() => throw new Exception($"Cannot convert {_typeName} to bool");

    public virtual Number ToNumber() => throw new Exception($"Cannot convert {_typeName} to number");
    public virtual IFunction ToFunction() => throw new Exception($"Cannot convert {_typeName} to function");

    public virtual SString ToSString() => throw new Exception($"Cannot convert {_typeName} to string");

    public virtual ITable ToTable() => throw new Exception($"Cannot convert {_typeName} to table");

    public abstract bool IsEqual(IValue other);
    
    public string GetTypeName() => _typeName;

    public virtual IValue Unpack() => this;
}