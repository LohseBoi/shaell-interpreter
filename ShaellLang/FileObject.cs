using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShaellLang;

public class FileObject : NativeTable, IValue
{
    private string _path;

    public FileObject(string path)
    {
        _path = path;
        GenerateFileValues();
    }
    public FileObject(IValue path) : this(path.ToSString().Val) { }

    private void GenerateFileValues()
    {
        SetValue("read", new RefValue(new NativeFunc( (x) =>
        {
            long[] args = x.Select(y => y.ToNumber().ToInteger()).ToArray();
            FileStream fs = new FileInfo(_path).OpenRead();
            fs.Seek(args[1], SeekOrigin.Begin);
            
            byte[] buffer = new byte[args[0]];
            fs.Read(buffer, 0, (int)args[0]);

            return new BString(buffer);
            //return new SString(System.Text.Encoding.Default.GetString(buffer)); //TODO: BString
        }, 2)));
        SetValue("delete", new RefValue(new NativeFunc( (x) =>
        {
            Console.WriteLine(_path);
            File.Delete(_path);
            return new SNull();
        }, 0)));
        SetValue("readToEnd", new RefValue(new NativeFunc( (x) =>
        {
            if (!File.Exists(_path))
                throw new Exception("No file");
            return new BString(File.ReadAllText(_path));
        }, 0)));
        SetValue("append", new RefValue(new NativeFunc( (x) =>
        {
            StreamWriter f = new FileInfo(_path).AppendText();
            f.WriteLine(x.ToArray()[0].ToSString().Val);
            f.Flush();
            return new SNull();
        }, 1)));
        SetValue("openReadStream", new RefValue(new NativeFunc( (x) =>
        {
            throw new NotImplementedException();

        }, 0)));
        SetValue("openWriteStream", new RefValue(new NativeFunc( (x) =>
        {
            throw new NotImplementedException();
        }, 0)));
        SetValue("size", new RefValue(new NativeFunc( (x) => new Number(new FileInfo(_path).Length), 2)));
        SetValue("exists", new RefValue(new NativeFunc( (x) => new SBool(File.Exists(_path)), 0)));
    }

    public bool ToBool() => true;

    public Number ToNumber() => throw new Exception("Type error, File cannot be convert to number.");

    public IFunction ToFunction()
    { //TODO: Ext. function call
        throw new Exception("Ext. func");
    }

    public SString ToSString() => new (_path);

    public ITable ToTable() => this;
}