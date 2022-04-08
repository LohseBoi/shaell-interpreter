using Antlr4.Runtime;
namespace ShaellLang;
using static ShaellParser;

public abstract class ShaellLexerBase : Lexer
{
    private int _templateDepth = 0;

    public ShaellLexerBase(ICharStream input) : base(input)
    {
        RuleNames = ShaellParser.ruleNames;
        GrammarFileName = "ShaellLexer";
    }

    public override string[] RuleNames { get; }
    public override string GrammarFileName { get; }

    public bool IsInTemplateString()
    {
        return _templateDepth > 0;
    }

    public void IncreaseDepth()
    {
        _templateDepth++;
    }
    public void DecreaseDepth()
    {
        _templateDepth--;
    }
}