using System.Collections.Generic;

namespace ShaellLang;

public class NativeTable : ITable
{
    private Dictionary<string, IValue> valueLookup;

    public NativeTable()
    {
        valueLookup = new Dictionary<string, IValue>();
    }

    public void SetValue(string key, IValue value)
    {
        valueLookup[key] = value;
    }

    public virtual RefValue GetValue(IValue key)
    {
        if (key is SString stringKey)
        {
            if (valueLookup.TryGetValue(stringKey.Val, out IValue val))
                return new RefValue(val);
        }
        return null;
    }

    public virtual void RemoveValue(IValue key)
    {
        return;
    }

    public IEnumerable<IValue> GetKeys() => throw new ShaellException(new SString($"Native Tables cannot be iterated."));
}