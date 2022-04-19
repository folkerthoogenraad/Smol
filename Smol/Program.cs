using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Smol.Compiler;
using System.Net;

namespace Smol
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Runtime runtime = new Runtime();

            runtime.RegisterCommand("print", (func, runtime) => {
                var value = runtime.PopValue();

                Console.WriteLine(value.ConvertString());
            });
            runtime.RegisterCommand("split", (func, runtime) => {
                var value = runtime.Pop().AsString();
                var on = func.GetString(runtime, "on");

                var splitted = value.Split(on);

                runtime.PushValue(splitted.Select(x => new SmolString(x)).ToArray());
            });
            runtime.RegisterCommand("trim", (func, runtime) => {
                var value = runtime.Pop().AsString();

                runtime.PushValue(value.Trim());
            });
            runtime.RegisterCommand("workingdir", (func, runtime) => {
                runtime.PushValue(Directory.GetCurrentDirectory());
            });
            runtime.RegisterCommand("setworkingdir", (func, runtime) => {
                var to = runtime.Pop().AsString();

                Directory.SetCurrentDirectory(to);
            });
            runtime.RegisterCommand("cd", (func, runtime) => {
                var to = runtime.Pop().AsString();

                Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + to);
            });
            runtime.RegisterCommand("dup", (func, runtime) => {
                var value = runtime.PopValue();

                runtime.PushValue(value);
                runtime.PushValue(value);
            });
            runtime.RegisterCommand("pop", (func, runtime) => {
                var value = runtime.PopValue();
            });
            runtime.RegisterCommand("clear", (func, runtime) => {
                Console.Clear();
            });
            runtime.RegisterCommand("clearvariables", (func, runtime) => {
                runtime.ClearVariables();
            });
            runtime.RegisterCommand("clearstack", (func, runtime) => {
                runtime.ClearVariables();
            });
            runtime.RegisterCommand("append", (func, runtime) => {
                var to = runtime.Pop().AsString();
                var from = runtime.Pop().AsString();

                runtime.PushValue(from + to);
            });
            runtime.RegisterCommand("invoke", (func, runtime) => {
                var lambda = runtime.Pop().AsLambda();

                var result = runtime.ExecuteScoped(lambda);

                if (result != null) runtime.PushValue(result);
            });
            runtime.RegisterCommand("parsesmol", (func, runtime) => {
                var input = runtime.Pop().AsString();

                Lexer lexer = new Lexer(input);
                Parser parser = new Parser(lexer.Lex());

                runtime.PushValue(parser.ConvertToLambda(parser.Parse()));
            });
            runtime.RegisterCommand("each", (func, runtime) => {
                var lambda = runtime.Pop().AsLambda();
                var array = runtime.Pop().AsArray();

                runtime.PushValue(array
                    .Select(x => runtime.ExecuteScoped(lambda, x))
                    .Where(x => x != null)
                    .ToArray());
            });

            runtime.RegisterCommand("join", (func, runtime) => {
                var array = runtime.Pop().AsArray();

                string with = string.Empty;

                if (func.HasParameter("with"))
                {
                    with = func.GetString(runtime, "with");
                }

                runtime.PushValue(string.Join(with, array.Select(x => x.ConvertString())));
            });
            runtime.RegisterCommand("range", (func, runtime) => {
                var array = runtime.Pop().AsArray();

                int from = 0;
                int to = array.Length - 1;

                if (func.HasParameter("from"))
                {
                    from = (int)func.GetNumber(runtime, "from");
                }
                if (func.HasParameter("to"))
                {
                    to = (int)func.GetNumber(runtime, "to");
                }

                if (to > array.Length)
                {
                    throw new ApplicationException("Index out of range.");
                }
                if (from < 0)
                {
                    throw new ApplicationException("Index out of range.");
                }
                if (from >= to)
                {
                    throw new ApplicationException("from > to");
                }

                int length = to - from;

                var newArray = new SmolValue[length];

                Array.Copy(array, from, newArray, 0, length);

                runtime.PushValue(newArray);
            });
            runtime.RegisterCommand("first", (func, runtime) => {
                var array = runtime.Pop().AsArray();

                runtime.PushValue(array[0]);
            });
            runtime.RegisterCommand("second", (func, runtime) => {
                var array = runtime.Pop().AsArray();

                runtime.PushValue(array[1]);
            });
            runtime.RegisterCommand("readfile", (func, runtime) => {
                var value = runtime.Pop().AsString();

                runtime.PushValue(File.ReadAllText(value));
            });

            runtime.RegisterCommand("asobject", (func, runtime) => {
                
                SmolObject obj = new SmolObject();

                foreach(var v in runtime.DeclaredVariables)
                {
                    obj.Set(v.Key, v.Value);
                }

                runtime.PushValue(obj);
            });

            runtime.RegisterCommand("writefile", (func, runtime) => {
                var value = runtime.Pop().AsString();

                var file = func.GetString(runtime, "path");

                File.WriteAllText(file, value);
            });
            runtime.RegisterCommand("downloadurl", (func, runtime) => {
                var url = runtime.Pop().AsString();

                using (var client = new WebClient())
                {
                    var result = client.DownloadString(url);

                    runtime.PushValue(result);
                }
            });
            runtime.RegisterCommand("listfiles", (func, runtime) => {
                var value = runtime.Pop().AsString();

                var files = Directory.GetFiles(value);

                runtime.PushValue(files.Select(x => new SmolString(x)).ToArray());
            });

            runtime.RegisterCommand("prompt", (func, runtime) => {
                if (func.HasParameter("text"))
                {
                    Console.WriteLine(func.GetString(runtime, "text"));
                }

                runtime.PushValue(Console.ReadLine());
            });

            runtime.RegisterCommand("parsenumber", (func, runtime) => {
                var value = runtime.Pop().AsString();

                runtime.PushValue(double.Parse(value));
            });

            runtime.RegisterCommand("help", (func, runtime) => {
                Console.WriteLine("Help will be provided later :)");
            });
            runtime.RegisterCommand("obj", (func, runtime) => {
                var obj = new SmolObject();

                obj.Set("test", new SmolString("Test123"));
                obj.Set("people", new SmolArray(new SmolString[] { new SmolString("Henk"), new SmolString("Gerard"), new SmolString("Peter") }));

                runtime.PushValue(obj);
            });
            runtime.RegisterCommand("dump", (func, runtime) => {
                int index = 0;
                foreach (var value in runtime.Stack)
                {
                    Console.WriteLine($"{index}: {value} ({value.Type})");
                    index++;
                }
            });
            runtime.RegisterCommand("exit", (func, runtime) => {
                Environment.Exit(0);
            });

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("$ ");
                Console.ForegroundColor = ConsoleColor.Blue;

                string line = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.White;

                Lexer lexer = new Lexer(line);
                Parser parser = new Parser(lexer.Lex());
                

                try
                {
                    var parsed = parser.Parse().ToArray();

                    foreach(var message in parser.Messages)
                    {
                        Console.WriteLine(message);
                    }

                    foreach (var command in parsed)
                    {
                        if(command == null)
                        {
                            Console.WriteLine("null command");
                            continue;
                        }

                        runtime.Execute(command);
                    }

                    // Maybe not?
                    runtime.ClearStack();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }
    }
}
