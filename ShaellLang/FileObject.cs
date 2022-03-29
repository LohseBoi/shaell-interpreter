using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShaellLang;

public class FileObject : IValue, ITable
{
    private Dictionary<string, RefValue> values;
    private string KeyValue(IKeyable key) => key.UniquePrefix + key.KeyValue;
    private string _path;

    public FileObject(string path)
    {
        _path = path;
        values = GenerateFileValues();
    }

    private Dictionary<string, RefValue> GenerateFileValues()
    {
        Dictionary<string, RefValue> _out = new Dictionary<string, RefValue>();
        
        _out.Add("read", new RefValue(new NativeFunc( (x) =>
        {
            if (!File.Exists(_path))
                throw new NotImplementedException();
            //return new BString();
            //File.READLALLA;
            return new SNull();
        }, 2)));
        _out.Add("readToEnd", new RefValue(new NativeFunc( (x) =>
        {
            if (!File.Exists(_path))
                throw new NotImplementedException();
                //return new BString();

                File.ReadAllText(_path); //TODO: use as argument for BString and return that
            return new SNull();
        }, 0)));
        _out.Add("append", new RefValue(new NativeFunc( (x) =>
        {
            //File.AppendAllText(_path, x.ToString());
            File.AppendAllLines(_path, x.Select( y => y.ToString()));
            return new SNull();
        }, 1)));
        _out.Add("delete", new RefValue(new NativeFunc( (x) =>
        {
            File.Delete(_path);
            return new SNull();
        }, 0)));
        _out.Add("openReadStream", new RefValue(new NativeFunc( (x) =>
        {
            throw new NotImplementedException();

        }, 0)));
        _out.Add("openWriteStream", new RefValue(new NativeFunc( (x) =>
        {
            throw new NotImplementedException();
        }, 0)));
        _out.Add("size", new RefValue(new NativeFunc( (x) => new Number(new FileInfo(_path).Length), 2)));
        _out.Add("exists", new RefValue(new NativeFunc( (x) => new SBool(File.Exists(_path)), 0)));
        return _out;
    }

    public bool ToBool() => File.Exists(_path);

    public Number ToNumber()
    {
        throw new NotImplementedException();
    }

    public IFunction ToFunction()
    {
        throw new Exception("Type error, File cannot be converted to function");
    }

    public SString ToSString() => new (_path);

    public ITable ToTable() => this;

    public RefValue GetValue(IKeyable key)
    {
        bool exists = values.TryGetValue(KeyValue(key), out RefValue value);
        if (exists)
            return value;
        throw new Exception($"No value with the given key ({key})");
    }

    public void RemoveValue(IKeyable key)
    {
        throw new System.NotImplementedException();
    }
}