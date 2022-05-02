using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Smol.Compiler;
using System.Net;
using System.Text.Json;
using System.Globalization;
using Smol.Values;

namespace Smol
{
    public class Program
    {
        public const string PRINT_STACK = "printstack";

        public static string ReadHighlightedString()
        {
            /*int index = Console.CursorLeft;

            StringBuilder sb = new StringBuilder();

            while (true)
            {
                var info = Console.ReadKey(true);

                if(info.Key == ConsoleKey.Enter)
                {
                    return sb.ToString();
                }
                else if(info.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);

                    Console.CursorVisible = false;
                    Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                    Console.Write(" ");
                    Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
                    Console.CursorVisible = true;
                }
                else
                {
                    sb.Append(info.KeyChar);
                    Console.Write(info.KeyChar);
                }

            }*/

            return Console.ReadLine();
        }

        public static void Main(string[] args)
        {
            SmolContext context = new SmolContext();

            Modules.System.Initialize(context);
            Modules.IO.Initialize(context);
            Modules.Arrays.Initialize(context);
            Modules.Maths.Initialize(context);
            Modules.Strings.Initialize(context);
            Modules.JSON.Initialize(context);

            SmolObject config = new SmolObject();
            config.Set(PRINT_STACK, new SmolBoolean(true));
            context.SetVariable("_config", config);


            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(Directory.GetCurrentDirectory());
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" $ ");
                Console.ForegroundColor = ConsoleColor.Blue;

                string line = ReadHighlightedString();

                Console.ForegroundColor = ConsoleColor.Gray;

                Lexer lexer = new Lexer(line);

                var tokens = lexer.Lex().ToArray();

                foreach (var message in lexer.Messages) Console.WriteLine(message);

                Parser parser = new Parser(tokens);
                
                try
                {
                    var parsed = parser.Parse().ToArray();

                    foreach(var message in parser.Messages) Console.WriteLine(message);
                    

                    foreach (var command in parsed)
                    {
                        if(command == null)
                        {
                            Console.WriteLine("null command");
                            continue;
                        }

                        context.Execute(command);
                    }

                    // TODO make sure you cannot lose config and shit
                    // write only objects?
                    if (config.Get(PRINT_STACK).AsBoolean())
                    {
                        int index = 0;
                        foreach (var value in context.Stack)
                        {
                            Console.WriteLine($"{index}: {value} ({value.Type})");
                            index++;
                        }
                    }

                    // Maybe not?
                    context.ClearStack();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }
    }
}
