using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Xml;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;


namespace ShaellLang
{

    class Program
    {
        static void Main(string[] args)
        {
            string indexer = "> ";
            /*
            if (args.Length < 1 ) {
                ExecutionVisitor executer = new ExecutionVisitor();
                executer.SetGlobal("$print", new NativeFunc(delegate(IEnumerable<IValue> _args)
                {
                    foreach (var value in _args)
                    {
                        Console.Write(value.ToSString().Val);
                    }
                    Console.WriteLine();
                    
                    return new SNull();
                }, 0));
                
                executer.SetGlobal("$T", TableLib.CreateLib());
            
                executer.SetGlobal("$debug_break", new NativeFunc(delegate(IEnumerable<IValue> args)
                {
                    Console.WriteLine("Debug break");
                    return new SNull();
                }, 0));

                string inputS = Readinput(indexer);
                while (!string.Equals(inputS, "::q", StringComparison.Ordinal))
                {
                    try
                    {
                        AntlrInputStream inputStream = new AntlrInputStream(inputS);
                        ShaellLexer shaellLexer = new ShaellLexer(inputStream);
                        shaellLexer.AddErrorListener(new ShaellLexerErrorListener());
                        CommonTokenStream commonTokenStream = new CommonTokenStream(shaellLexer);
                        ShaellParser shaellParser = new ShaellParser(commonTokenStream);
            
                        ShaellParser.ProgContext progContext = shaellParser.prog();

                        executer.Visit(progContext);
                    }
                    catch (SyntaxErrorException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    inputS = Readinput(indexer);
                }
                
                return;
            }
            */
            Func<string> input = args.Length < 1 ? () => Readinput(indexer) : () => File.ReadAllText(args[0]);
            //var filename = args[0];
            //var content = File.ReadAllText(filename);
            
            ExecutionVisitor executer = new ExecutionVisitor();
            executer.SetGlobal("$print", new NativeFunc(delegate(IEnumerable<IValue> args)
            {
                foreach (var value in args)
                    Console.Write(value.ToSString().Val);
                Console.WriteLine();

                return new SNull();
            }, 0));
                
            executer.SetGlobal("$T", TableLib.CreateLib());
            
            executer.SetGlobal("$debug_break", new NativeFunc(delegate(IEnumerable<IValue> args)
            {
                Console.WriteLine("Debug break");
                return new SNull();
            }, 0));

            do
            {
                try
                {
                    AntlrInputStream inputStream = new AntlrInputStream(input());
                    ShaellLexer shaellLexer = new ShaellLexer(inputStream);
                    shaellLexer.AddErrorListener(new ShaellLexerErrorListener());
                    CommonTokenStream commonTokenStream = new CommonTokenStream(shaellLexer);
                    ShaellParser shaellParser = new ShaellParser(commonTokenStream);

                    ShaellParser.ProgContext progContext = shaellParser.prog();

                    executer.Visit(progContext);
                }
                catch (SyntaxErrorException e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (args.Length < 1);
            
        }

        private static string Readinput(string indexer)
        {
            List<char> input = new List<char>();
            Console.Write(indexer);
            uint index = 0;
            Console.TreatControlCAsInput = true;
            while (Console.ReadKey() is ConsoleKeyInfo key && key is not {Key: ConsoleKey.Enter})
            {
                if (key.Modifiers == ConsoleModifiers.Control)
                {
                    if (key.Key == ConsoleKey.C)
                    {
                        Console.WriteLine();
                        input = new List<char>();
                        index = 0;
                        Console.Write(indexer);
                        continue;
                    }
                    if (key.Key == ConsoleKey.G)
                    {
                        if (!input.Any() && index == 0)
                            Environment.Exit(0);
                        Console.WriteLine();
                        Console.WriteLine("Input <CTRL+G> one more time and see what happens, punk!");
                        input = new List<char>();
                        index = 0;
                        Console.Write(indexer);
                        continue;
                    }
                }
                
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (index != 0)
                        {
                            index--;
                            Console.CursorLeft--;
                        }

                        break;
                    case ConsoleKey.RightArrow:
                        if (index != input.Count)
                        {
                            Console.CursorLeft++;
                            index++;
                        }
                        break;

                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        continue;
                    
                    case ConsoleKey.Backspace:
                        if (index != 0)
                        {
                            index--;
                            input.RemoveAt((int) index);
                            Console.CursorLeft--;
                        }

                        break;
                    default:
                        input.Insert((int) index, key.KeyChar);
                        index++;
                        break;
                }

                var (left, top) = Console.GetCursorPosition();
                Console.CursorLeft = indexer.Length;
                Console.Write(new string(' ', Console.BufferWidth - indexer.Length));
                Console.CursorLeft = indexer.Length;
                Console.CursorTop = top;
                Console.Write(input.ToArray());
                Console.SetCursorPosition(left, top);
            }
            Console.WriteLine();
            return new string(input.ToArray());
        }
    }
}
