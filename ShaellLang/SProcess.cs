using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ShaellLang;

public class SProcess : BaseValue, IFunction
{
    public Process Process = new Process();
    public SProcess LeftProcess;
    public string Stdin = null;
    public bool Executed { get; private set; }
    public SProcess(string file) 
        : base("process")
    {
        IPathFinder pathFinder;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            pathFinder = new WindowsPathFinder();
            Process.StartInfo.FileName = pathFinder.GetAbsolutePath(file);
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            pathFinder = new UnixPathFinder();
            Process.StartInfo.FileName = pathFinder.GetAbsolutePath(file);
        }
    }

    public void AddArguments(IEnumerable<IValue> args)
    {
        foreach (var arg in args)
        {
            AddArg(arg.ToSString().Val);
        }
    }

    public void AddPipeProcess(SProcess process)
    {
        LeftProcess = process;
    }

    private void AddArg(string str) => Process.StartInfo.ArgumentList.Add(str);
    public void Dispose() => Process.Dispose();

    private JobObject Run(Process process, string stdin)
    {
        if (!Executed)
        {
            Executed = true;
            return JobObject.Factory.StartProcess(process, stdin);
        }

        return null;
    }

    public IValue Call(IEnumerable<IValue> args)
    {
        AddArguments(args);
        return Run(Process, Stdin);
    }

    public JobObject Execute(SProcess parentProc)
    {
        JobObject jo = LeftProcess?.Execute(this).ToJobObject();
        jo = Run(Process, jo?.ToString()).ToJobObject();

        if(parentProc != null)
            parentProc.Stdin = jo?.ToString();
        return jo;
    }
    
    public JobObject Execute()
    {
        JobObject jo = LeftProcess?.Execute(this).ToJobObject();
        jo = Run(Process, jo?.ToString()).ToJobObject();
        
        return jo;
    }
    
    public override IFunction ToFunction() => this;
    public override SProcess ToSProcess() => this;

    public override ITable ToTable() => Execute().ToTable();

    public override bool IsEqual(IValue other)
    {
        return other == this;
    }

    public uint ArgumentCount => (uint)Process.StartInfo.ArgumentList.Count;
}