using System;
using System.Collections.Generic;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;


namespace ShaellLang
{

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
            var executer = new ExecutionVisitor();
            executer.SetGlobal("$print", new NativeFunc(delegate(ICollection<IValue> values)
            {
                foreach (var value in values)
                {
                    Console.Write(value.ToSString().Val);
                }
                Console.WriteLine();

                return new SNull();
            }, 0));
            executer.Visit(progContext);
            
        }
    }
}
