using Smol.Compiler;
using Smol.Values;
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
        public static void Initialize(SmolContext context)
        {
            // ======================================== //
            // Console printing etc (maybe not for system?)
            // ======================================== //
            context.RegisterCommand("print", (func, context) => {
                var value = context.PopValue();

                Console.WriteLine(value.ConvertString());
            });
            context.RegisterCommand("prompt", (func, context) => {
                if (func.HasParameter("text"))
                {
                    Console.WriteLine(func.GetString(context, "text"));
                }

                context.PushValue(Console.ReadLine());
            });
            context.RegisterCommand("clear", (func, context) => {
                Console.Clear();
            });
            // Debugging
            context.RegisterCommand("dump", (func, context) => {
                int index = 0;
                foreach (var value in context.Stack)
                {
                    Console.WriteLine($"{index}: {value} ({value.Type})");
                    index++;
                }
            });
            context.RegisterCommand("exit", (func, context) => {
                Environment.Exit(0);
            });

            // ======================================== //
            // Working directory and navigation
            // ======================================== //
            context.RegisterCommand("workingdir", (func, context) => {
                context.PushValue(Directory.GetCurrentDirectory());
            });
            context.RegisterCommand("setworkingdir", (func, context) => {
                var to = context.Pop().AsString();

                Directory.SetCurrentDirectory(to);
            });
            context.RegisterCommand("cd", (func, context) => {
                var to = context.Pop().AsString();

                Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + to);
            });

            // ======================================== //
            // Stack manipulation
            // ======================================== //
            context.RegisterCommand("dup", (func, context) => {
                var value = context.PopValue();

                context.PushValue(value);
                context.PushValue(value);
            });
            context.RegisterCommand("pop", (func, context) => {
                var value = context.PopValue();
            });
            context.RegisterCommand("clearvariables", (func, context) => {
                context.ClearVariables();
            });
            context.RegisterCommand("clearstack", (func, context) => {
                context.ClearVariables();
            });
            context.RegisterCommand("this", (func, context) => {
                context.PushValue(context.This);
            });

            // ======================================== //
            // Lambda stuff
            // ======================================== //
            context.RegisterCommand("invoke", (func, context) => {
                var lambda = context.Pop().AsLambda();

                var @this = context.This;

                if (func.HasParameter("on"))
                {
                    @this = func.GetValue(context, "on").AsObject();
                }

                var result = context.ExecuteScopedOn(lambda, @this);

                if (result != null) context.PushValue(result);
            });

            // ======================================== //
            // smol
            // ======================================== //
            context.RegisterCommand("smol_parse", (func, context) => {
                var input = context.Pop().AsString();

                Lexer lexer = new Lexer(input);
                Parser parser = new Parser(lexer.Lex());

                context.PushValue(parser.ConvertToLambda(parser.Parse()));
            });
            context.RegisterCommand("smol_serialize", (func, context) => {
                var value = context.Pop();

                if (value.IsObject)
                {
                    var obj = value.AsObject();
                    context.PushValue(obj.ToFormattedString());
                }
                else
                {
                    context.PushValue(value.ToString());
                }
            });


            // ======================================== //
            // Other system calls
            // ======================================== //
            context.RegisterCommand("help", (func, context) => {
                Console.WriteLine("Help will be provided later :)");
            });

            context.RegisterCommand("to_string", (func, context) => {
                var value = context.Pop();

                context.PushValue(value.ConvertString());
            });
            context.RegisterCommand("new", (func, context) => {
                context.PushValue(new SmolObject());
            });

            context.RegisterCommand("parse_number", (func, context) => {
                var value = context.Pop().AsString();

                context.PushValue(double.Parse(value, CultureInfo.InvariantCulture));
            });
            context.RegisterCommand("parse_boolean", (func, context) => {
                var value = context.Pop().AsString();

                context.PushValue(bool.Parse(value));
            });
        }
    }
}
