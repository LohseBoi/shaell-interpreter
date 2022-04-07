using System;
using Antlr4.Runtime;

namespace ShaellLang;

public class SemanticError : Exception
{
    private IToken _offendingTokenStart;
    private IToken _offendingTokenEnd;

    public SemanticError(string message, IToken offendingTokenStart)
        : base(message)
    {
        _offendingTokenStart = offendingTokenStart;
    }
    
    public SemanticError(string message, IToken offendingTokenStart, IToken offendingTokenEnd)
        : base(message)
    {
        _offendingTokenStart = offendingTokenStart;
        _offendingTokenEnd = offendingTokenEnd;
    }

    public override string ToString()
    {
        if (_offendingTokenEnd == null)
        {
            return $"Semantic error at token {_offendingTokenStart.Line}:{_offendingTokenStart.Column}: {Message}";
        }
        return $"Semantic error at token spanning from {_offendingTokenStart.Line}:{_offendingTokenStart.Column} - {_offendingTokenEnd.Line}:{_offendingTokenEnd.Column} {Message}";
    }
}