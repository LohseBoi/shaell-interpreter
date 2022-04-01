using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class NumberTable : NativeTable
{
    private static NumberTable instance = new NumberTable();
    public Number Number;
    
    private NumberTable()
    {
        this.SetValue("sqrt", new NativeFunc(sqrtCallHandler, 0));
        this.SetValue("floor", new NativeFunc(floorCallHandler, 0));
        this.SetValue("ceil", new NativeFunc(ceilCallHandler,0));
        this.SetValue("log2", new NativeFunc(log2CallHandler,0));
        this.SetValue("log", new NativeFunc(logCallHandler,0));
    }

    public static NumberTable getInstance()
    {
        return instance;
    }
    
    private IValue sqrtCallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Sqrt(Number.ToFloating()));
    }

    private IValue floorCallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Floor(Number.ToFloating()));
    }

    private IValue ceilCallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Ceiling(Number.ToFloating()));
    }

    private IValue log2CallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Log2(Number.ToFloating()));
    }

    private IValue logCallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Log(Number.ToFloating(), args.ToArray()[0].ToNumber().ToFloating()));
    }
}