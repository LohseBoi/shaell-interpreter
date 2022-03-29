using System.Collections.Generic;

namespace ShaellLang;

public class NativeTable : ITable
{
    private Dictionary<string, IValue> _functionLookup;

    public NativeTable()
    {
        _functionLookup = new Dictionary<string, IValue>();
    }

    protected void SetValue(string key, IValue value)
    {
        _functionLookup[key] = value;
    }

    public virtual RefValue GetValue(IKeyable key)
    {
        if (key is SString)
        {
            if (_functionLookup.TryGetValue(key.KeyValue, out IValue val))
            {
                return new RefValue(val);
            }
        }

        return null;
    }

    public void RemoveValue(IKeyable key)
    {
        return;
    }
}