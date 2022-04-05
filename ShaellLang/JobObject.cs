using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ShaellLang;

public class JobObject : BaseValue
{
    private readonly Process _process;
    private NativeTable _nativeTable;
    public JobObject(IValue value) : this()
    {
        _nativeTable = new NativeTable();
        _nativeTable.SetValue("value", value);
    }
    
    private IValue Value
    {
        get => _nativeTable.GetValue(new SString("value"));
        set => _nativeTable.SetValue("value", value);
    }

    public IValue Out
    {
        get => _nativeTable.GetValue(new SString("out"));
        private set => _nativeTable.SetValue("out", value);
    }
    
    private IValue Err
    {
        get => _nativeTable.GetValue(new SString("err"));
        set => _nativeTable.SetValue("err", value);
    }

    private JobObject(Process process) : this()
    {
        _process = process;
    }

    private JobObject() : base("jobobject")
    {
        _nativeTable = new NativeTable();
        _nativeTable.SetValue("out", new SNull());
        _nativeTable.SetValue("value", new SNull());
        _nativeTable.SetValue("err", new SNull());
    }

    public static class Factory
    {
        public static JobObject StartProcess(Process process, string stdin)
        {
            var jo = new JobObject(process);
            process.Exited += (sender, args) => jo.Value = new Number(process.ExitCode);
            process.EnableRaisingEvents = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            if(stdin is not null)
                process.StandardInput.Write(stdin);
            process.WaitForExit();
            jo.Out = new SString(process.StandardOutput.ReadToEnd());
            jo.Err = new SString(process.StandardError.ReadToEnd());

            return jo;
        }
    }

    public override bool ToBool()
    {
        return Value.ToBool();
    }

    public override Number ToNumber()
    {
        return Value.ToNumber();
    }

    public override IFunction ToFunction()
    {
        return Value.ToFunction();
    }

    public override SString ToSString()
    {
        return Value.ToSString();
    }

    public override ITable ToTable()
    {
        return _nativeTable;
    }

    public override JobObject ToJobObject()
    {
        return this;
    }

    public override bool IsEqual(IValue other)
    {
        return this == other;
    }

    public override string ToString()
    {
        return Out.ToString();
    }
}