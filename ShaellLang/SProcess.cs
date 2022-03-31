using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Threading.Tasks;

namespace ShaellLang;

public class SProcess : BaseValue, IFunction
{
    private Process _process = new Process();
    public SProcess(string file) 
        : base("process")
    {
        _process.StartInfo.FileName = file;
    }

    private void AddArguments(IEnumerable<IValue> args)
    {
        foreach (var arg in args)
        {
            AddArg(arg.ToSString().Val);
        }
    }

    private void AddArg(string str) => _process.StartInfo.ArgumentList.Add(str);
    public void Dispose() => _process.Dispose();

    private JobObject Run(Process process)
    {
        return JobObject.Factory.StartProcess(process);
    }

    public IValue Call(IEnumerable<IValue> args)
    {
        AddArguments(args);
        return Run(_process);
    }
    
    public override IFunction ToFunction() => this;
    public override bool IsEqual(IValue other)
    {
        return other == this;
    }

    public uint ArgumentCount { get; }
}