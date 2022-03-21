using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;


namespace ShaellLang
{
    class ProgVisitor : ShaellBaseVisitor<object> {
        public override object VisitProg(ShaellParser.ProgContext context)
        {            
            Console.WriteLine("Jeg besøger en prog");
            return null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 ) {
                Console.WriteLine("Expected first argument to be filename");
                return;
            }
            var filename = args[0];

            var content = File.ReadAllText(filename);

            AntlrInputStream inputStream = new AntlrInputStream(content);
            ShaellLexer shaellLexer = new ShaellLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(shaellLexer);
            ShaellParser shaellParser = new ShaellParser(commonTokenStream);
            
            ShaellParser.ProgContext progContext = shaellParser.prog();
            var progVisitor = new ProgVisitor();
            progVisitor.Visit(progContext);
            
        }
    }
}
