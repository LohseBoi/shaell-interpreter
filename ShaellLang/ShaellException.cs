using System;

namespace ShaellLang;

public class ShaellException : Exception
{
    public IValue ExceptionValue { get; }
    
    public ShaellException(IValue exceptionValue)
    {
        ExceptionValue = exceptionValue;
    }
}