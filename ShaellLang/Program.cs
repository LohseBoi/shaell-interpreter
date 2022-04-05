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




            bool interactivemode = args.Length < 1;
            
            //string indexer = "> ";
            Func<string> indexer = () => $"{Directory.GetCurrentDirectory().Split('/').Last()} $ ";
            Func<string> input;
            if (!interactivemode)
                input = () => File.ReadAllText(args[0]);
            else
            {
                input = () =>
                {
                    input = () => ReadInput(indexer());
                    return File.ReadAllText("testscripts/.shællrc"); //TODO: Temp, shouyld be "~/.shællrc"
                };
                //TODO: load .shæll_history
            }
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
            } while (interactivemode);
            
        }

        private static string ReadInput(string indexer)
        {
            List<char> input = new List<char>();
            Console.Write(indexer);
            int inputIndex = 0;
            int cmdIndex = 0;
            var cmdHistory = File.ReadLines("testscripts/.shæll_history").Reverse()
                .Select(x => x.Substring(x.IndexOf(':') + 1)).ToList(); //TODO: temp, should be ~/.shæll_history
            cmdHistory.Insert(0,"");
            Console.TreatControlCAsInput = true;
            while (Console.ReadKey() is ConsoleKeyInfo key && key is not {Key: ConsoleKey.Enter})
            {
                if (key.Modifiers == ConsoleModifiers.Control)
                {
                    if (key.Key == ConsoleKey.C)
                    {
                        Console.WriteLine();
                        input = new List<char>();
                        inputIndex = 0;
                        Console.Write(indexer);
                        continue;
                    }

                    if (key.Key == ConsoleKey.G)
                    {
                        if (!input.Any() && inputIndex == 0)
                            Environment.Exit(0);
                        Console.WriteLine();
                        Console.WriteLine("Input <CTRL+G> one more time and see what happens, punk!");
                        input = new List<char>();
                        inputIndex = 0;
                        Console.Write(indexer);
                        continue;
                    }
                }

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (inputIndex != 0)
                        {
                            inputIndex--;
                            Console.CursorLeft--;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (inputIndex != input.Count)
                        {
                            Console.CursorLeft++;
                            inputIndex++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        cmdIndex = cmdHistory.Count - 1 <= cmdIndex ? cmdIndex : cmdIndex + 1;
                        input = cmdHistory[cmdIndex].ToCharArray().ToList();
                        Console.CursorLeft = indexer.Length + input.Count;
                        inputIndex = input.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        cmdIndex = cmdIndex <= 0 ? 0 : cmdIndex - 1;
                        input = cmdHistory[cmdIndex].ToCharArray().ToList();
                        Console.CursorLeft = indexer.Length + input.Count;
                        inputIndex = input.Count;
                        break;
                    case ConsoleKey.Backspace:
                        if (inputIndex != 0)
                        {
                            inputIndex--;
                            input.RemoveAt(inputIndex);
                            Console.CursorLeft--;
                        }
                        break;
                    case ConsoleKey.Delete:
                        if (inputIndex < input.Count)
                            input.RemoveAt(inputIndex);
                        break;
                    
                    default:
                        if (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || char.IsSymbol(key.KeyChar))
                        {
                            input.Insert(inputIndex, key.KeyChar);
                            inputIndex++;
                        }
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

            string _out = new string(input.ToArray());

            if (_out.Length > 0)
                File.AppendAllLines("testscripts/.shæll_history", new[] { $"{DateTime.Now:G}:{_out}" }); //TODO: temp, should be ~/.shæll_history
            Console.WriteLine();
            return _out;
        }
    }
}
