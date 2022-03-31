using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class TableLib
{
    public static IValue CreateLib()
    {
        var userTable = new UserTable();
        userTable
            .GetValue(new SString("insert"))
            .Set(new NativeFunc(InsertFunc, 2));
        
        userTable
            .GetValue(new SString("length"))
            .Set(new NativeFunc(LengthFunc, 1));

        return userTable;
    }
    private static IValue InsertFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();
        if (argArr.Length > 0 && argArr[0] is UserTable userTable)
        {
            return userTable.InsertFunc(args.Skip(1));
        }
        throw new Exception("error: no table supplied");
    }
    
    private static IValue LengthFunc(IEnumerable<IValue> args)
    {
        var argArr = args.ToArray();
        if (argArr.Length > 0 && argArr[0] is UserTable userTable)
        {
            return userTable.LengthFunc(args.Skip(1));
        }
        throw new Exception("error: no table supplied");
    }
}