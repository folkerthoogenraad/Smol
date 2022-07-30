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

namespace Smol.Runtime
{
    public class Program
    {
        public const string PRINT_STACK = "printstack";

        public static string ReadHighlightedString()
        {
            StringBuilder sb = new StringBuilder();

            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            int index = 0;

            while (true)
            {
                var info = Console.ReadKey(true);

                if (info.Key == ConsoleKey.Enter)
                {
                    Console.Write("\n");
                    return sb.ToString();
                }
                else if (info.Key == ConsoleKey.Backspace && index > 0)
                {
                    sb.Remove(index - 1, 1);
                    index--;
                }
                else if (info.Key == ConsoleKey.Delete && index < sb.Length)
                {
                    sb.Remove(index, 1);
                }
                else if (info.Key == ConsoleKey.LeftArrow)
                {
                    index--;
                }
                else if (info.Key == ConsoleKey.RightArrow)
                {
                    index++;
                }
                else if (IsLegalChar(info.KeyChar))
                {
                    sb.Insert(index, info.KeyChar);
                    index++;
                }

                if (index < 0) { index = 0; }
                if (index > sb.Length) index = sb.Length;

                Console.CursorVisible = false;
                Console.SetCursorPosition(left, top);

                Lexer lexer = new Lexer(sb.ToString());

                var tokens = lexer.Lex().ToArray();

                foreach (var token in tokens)
                {
                    switch (token.Type)
                    {
                        case Token.TokenType.String:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case Token.TokenType.At:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case Token.TokenType.Unknown:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case Token.TokenType.Variable:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case Token.TokenType.Param:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.Write(token.Data);
                }
                Console.Write(" ");

                Console.SetCursorPosition(left + index, top);
                Console.CursorVisible = true;
            }
        }

        public static bool IsLegalChar(char c)
        {
            return (c >= 'a' && c <= 'z')
               || (c >= 'A' && c <= 'Z')
               || (c >= '0' && c <= '9')
               || (c == ' ')
               || (c == '(')
               || (c == ')')
               || (c == '{')
               || (c == '}')
               || (c == ':')
               || (c == '/')
               || (c == '[')
               || (c == ']')
               || (c == '@')
               || (c == '?')
               || (c == '"')
               || (c == '.')
               || (c == '-')
               || (c == '>')
               || (c == '$')
               || (c == '_');
        }

        public static void Main(string[] args)
        {
            SmolRuntime runtime = new SmolRuntime();

            SmolObject config = new SmolObject();
            config.Set(PRINT_STACK, new SmolBoolean(true));
            runtime.Context.SetVariable("_config", config);

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(Directory.GetCurrentDirectory());
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" $ ");

                string line = ReadHighlightedString();

                Console.ForegroundColor = ConsoleColor.Gray;

                Lexer lexer = new Lexer(line);

                var tokens = lexer.Lex().ToArray();

                foreach (var message in lexer.Messages) Console.WriteLine(message);

                Parser parser = new Parser(tokens);

                try
                {
                    var parsed = parser.Parse().ToArray();

                    foreach (var message in parser.Messages) Console.WriteLine(message);


                    foreach (var command in parsed)
                    {
                        if (command == null)
                        {
                            Console.WriteLine("null command");
                            continue;
                        }

                        runtime.Context.Execute(command);
                    }

                    // TODO make sure you cannot lose config and shit
                    // write only objects?
                    if (config.Get(PRINT_STACK).AsBoolean())
                    {
                        int index = 0;
                        foreach (var value in runtime.Context.Stack)
                        {
                            Console.WriteLine($"{index}: {value} ({value.Type})");
                            index++;
                        }
                    }

                    // Maybe not?
                    runtime.Context.ClearStack();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}
