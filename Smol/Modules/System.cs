using Smol.Compiler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public static class System
    {
        public static void Initialize(Runtime runtime)
        {
            // ======================================== //
            // Console printing etc (maybe not for system?)
            // ======================================== //
            runtime.RegisterCommand("print", (func, runtime) => {
                var value = runtime.PopValue();

                Console.WriteLine(value.ConvertString());
            });
            runtime.RegisterCommand("prompt", (func, runtime) => {
                if (func.HasParameter("text"))
                {
                    Console.WriteLine(func.GetString(runtime, "text"));
                }

                runtime.PushValue(Console.ReadLine());
            });
            runtime.RegisterCommand("clear", (func, runtime) => {
                Console.Clear();
            });
            // Debugging
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

            // ======================================== //
            // Working directory and navigation
            // ======================================== //
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

            // ======================================== //
            // Stack manipulation
            // ======================================== //
            runtime.RegisterCommand("dup", (func, runtime) => {
                var value = runtime.PopValue();

                runtime.PushValue(value);
                runtime.PushValue(value);
            });
            runtime.RegisterCommand("pop", (func, runtime) => {
                var value = runtime.PopValue();
            });
            runtime.RegisterCommand("clearvariables", (func, runtime) => {
                runtime.ClearVariables();
            });
            runtime.RegisterCommand("clearstack", (func, runtime) => {
                runtime.ClearVariables();
            });
            runtime.RegisterCommand("this", (func, runtime) => {
                runtime.PushValue(runtime.This);
            });

            // ======================================== //
            // Lambda stuff
            // ======================================== //
            runtime.RegisterCommand("invoke", (func, runtime) => {
                var lambda = runtime.Pop().AsLambda();

                var @this = runtime.This;

                if (func.HasParameter("on"))
                {
                    @this = func.GetValue(runtime, "on").AsObject();
                }

                var result = runtime.ExecuteScopedOn(lambda, @this);

                if (result != null) runtime.PushValue(result);
            });

            // ======================================== //
            // smol
            // ======================================== //
            runtime.RegisterCommand("smol_parse", (func, runtime) => {
                var input = runtime.Pop().AsString();

                Lexer lexer = new Lexer(input);
                Parser parser = new Parser(lexer.Lex());

                runtime.PushValue(parser.ConvertToLambda(parser.Parse()));
            });
            runtime.RegisterCommand("smol_serialize", (func, runtime) => {
                var value = runtime.Pop();

                if (value.IsObject)
                {
                    var obj = value.AsObject();
                    runtime.PushValue(obj.ToFormattedString());
                }
                else
                {
                    runtime.PushValue(value.ToString());
                }
            });


            // ======================================== //
            // Other system calls
            // ======================================== //
            runtime.RegisterCommand("help", (func, runtime) => {
                Console.WriteLine("Help will be provided later :)");
            });

            runtime.RegisterCommand("to_string", (func, runtime) => {
                var value = runtime.Pop();

                runtime.PushValue(value.ConvertString());
            });
            runtime.RegisterCommand("new", (func, runtime) => {
                runtime.PushValue(new SmolObject());
            });

            runtime.RegisterCommand("parse_number", (func, runtime) => {
                var value = runtime.Pop().AsString();

                runtime.PushValue(double.Parse(value, CultureInfo.InvariantCulture));
            });
            runtime.RegisterCommand("parse_boolean", (func, runtime) => {
                var value = runtime.Pop().AsString();

                runtime.PushValue(bool.Parse(value));
            });
        }
    }
}
