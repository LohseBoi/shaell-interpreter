using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class NumberTable : NativeTable
{
    private static readonly NumberTable _instance = new();
    private static Number _number;
    
    private NumberTable()
    {
        SetValue("sqrt", new NativeFunc(SqrtCallHandler, 0));
        SetValue("floor", new NativeFunc(FloorCallHandler, 0));
        SetValue("ceil", new NativeFunc(CeilCallHandler,0));
        SetValue("log2", new NativeFunc(Log2CallHandler,0));
        SetValue("log", new NativeFunc(LogCallHandler,0));
    }

    public static NumberTable GetInstance(Number caller)
    {
        _number = caller;
        return _instance;
    }
    
    private static IValue SqrtCallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Sqrt(_number.ToFloating()));
    }

    private static IValue FloorCallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Floor(_number.ToFloating()));
    }

    private static IValue CeilCallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Ceiling(_number.ToFloating()));
    }

    private static IValue Log2CallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Log2(_number.ToFloating()));
    }

    private static IValue LogCallHandler(IEnumerable<IValue> args)
    {
        return new Number(Math.Log(_number.ToFloating(), args.ToArray()[0].ToNumber().ToFloating()));
    }
}