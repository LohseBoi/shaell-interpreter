using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;

namespace ShaellLang
{

    class Program
    {
        private static PlatformID OS;
        static void Main(string[] args)
        {
            OS = Environment.OSVersion.Platform;
            bool interactivemode = args.Length < 1; //TODO: Adjust for Shæll-flags, and not just the users scripts flags

            Func<string> indexer = () => $"{Directory.GetCurrentDirectory().Split(new [] { '/', '\\' }).Last()} $ ";
            Func<string> input;
            if (!interactivemode)
                input = () => File.ReadAllText(args[0]);
            else
            {
                input = () =>
                {
                    input = () => ReadInputLinux(indexer());
                    //input = OS is PlatformID.Unix or PlatformID.MacOSX ? () => ReadInputLinux(indexer()) : () => ReadInputWin(indexer());
                    Console.TreatControlCAsInput = true;
                    string home = OS is PlatformID.Unix or PlatformID.MacOSX
                        ? Environment.GetEnvironmentVariable("HOME")
                        : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
                    if (File.Exists($"{home}/.shællrc"))
                        return File.ReadAllText($"{home}/.shællrc");
                    return input();
                };
                //TODO: load .shæll_history
            }

            var executer = interactivemode ? new ExecutionVisitor() : new ExecutionVisitor(args[1..]);

            executer.SetGlobal("$print", new NativeFunc(delegate(IEnumerable<IValue> innerArgs)
            {
                foreach (var value in innerArgs)
                    Console.Write(value.ToSString().Val);
                Console.WriteLine();

                return new SNull();
            }, 0));

            executer.SetGlobal("$T", TableLib.CreateLib());

            executer.SetGlobal("$A", TestLib.CreateLib());

            executer.SetGlobal("$debug_break", new NativeFunc(delegate
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
                // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            } while (interactivemode);

        }

        private static string ReadInputLinux(string indexer)
        {
            List<char> input = new List<char>();
            Console.Write(indexer);
            int inputIndex = 0;
            int cmdIndex = 0;
            string home = OS is PlatformID.Unix or PlatformID.MacOSX ? Environment.GetEnvironmentVariable("HOME") : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");;
            List<string> cmdHistory = new List<string> { "" };
            if (File.Exists($"{home}/.shæll_history"))
                cmdHistory.AddRange(File.ReadLines($"{home}/.shæll_history").Reverse()
                    .Select(x => x.Substring(x.IndexOf(':') + 1)));
            
            while (Console.ReadKey() is var key && key is not {Key: ConsoleKey.Enter})
            {
                if (key.Modifiers == ConsoleModifiers.Control)
                {
                    if (key.Key == ConsoleKey.C) //TODO: Stop Process? Should be moved away from here when implemented
                    {
                        Console.WriteLine();
                        input = new List<char>();
                        inputIndex = 0;
                        Console.Write(indexer);
                        continue;
                    }
                    /*
                    if (key.Key == ConsoleKey.Z) //TODO: Suspend Process? Should be moved away from here when implemented
                    {
                    }
                    */
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
                            Console.CursorLeft--;
                            inputIndex--;
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
                        if (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || char.IsSymbol(key.KeyChar) || char.IsWhiteSpace(key.KeyChar))
                        {
                            input.Insert(inputIndex, key.KeyChar);
                            inputIndex++;
                        }
                        break;
                }
                
                //var (left, top) = Console.GetCursorPosition();
                //Console.CursorLeft = indexer.Length;
                //Console.Write(new string(' ', Console.BufferWidth - indexer.Length));
                Console.CursorLeft = indexer.Length;
                //Console.CursorTop = top;
                Console.Write(input.ToArray());
                Console.CursorLeft = indexer.Length + inputIndex;
            }
            
            string @out = new string(input.ToArray());
            
            if (@out.Length > 0)
                SaveCmd(home, @out);
            Console.WriteLine();
            return @out;
        }
        private static string ReadInputWin(string indexer)
        {
            List<char> input = new List<char>();
            Console.Write(indexer);
            int inputIndex = 0;
            int cmdIndex = 0;
            string home = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            List<string> cmdHistory = new List<string> { "" };
            if (File.Exists($"{home}/.shæll_history"))
                cmdHistory.AddRange(File.ReadLines($"{home}/.shæll_history").Reverse()
                    .Select(x => x.Substring(x.IndexOf(':') + 1)));
            
            
            while (Console.ReadKey() is var key && key is not {Key: ConsoleKey.Enter})
            {
                if (key.Modifiers == ConsoleModifiers.Control)
                {
                    if (key.Key == ConsoleKey.C) //TODO: Stop Process? Should be moved away from here when implemented
                    {
                        Console.WriteLine();
                        input = new List<char>();
                        inputIndex = 0;
                        Console.Write(indexer);
                        continue;
                    }
                    /*
                    if (key.Key == ConsoleKey.Z) //TODO: Suspend Process? Should be moved away from here when implemented
                    {
                    }
                    */
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
                            Console.CursorLeft--;
                            inputIndex--;
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
                        if (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || char.IsSymbol(key.KeyChar) || char.IsWhiteSpace(key.KeyChar))
                        {
                            input.Insert(inputIndex, key.KeyChar);
                            inputIndex++;
                        }
                        break;
                }
                
                //var (left, top) = Console.GetCursorPosition();
                Console.CursorLeft = indexer.Length;
                Console.Write(new string(' ', Console.BufferWidth - indexer.Length));
                Console.CursorLeft = indexer.Length;
                //Console.CursorTop = top;
                Console.Write(input.ToArray());
                //Console.SetCursorPosition(left, top);
            
            }
            
            string @out = new string(input.ToArray());
            
            if (@out.Length > 0)
                SaveCmd(home, @out);
            Console.WriteLine();
            return @out;
        }
        static void SaveCmd(string path, string cmd) => File.AppendAllText($"{path}/.shæll_history", $"{DateTime.Now:MM/dd/yyyy HH.mm.ss}:{cmd}\n");
    }
}
