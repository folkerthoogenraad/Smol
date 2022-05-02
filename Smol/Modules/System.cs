using Smol.Compiler;
using Smol.Expressions;
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

            context.RegisterCommand("value_type", (func, context) => {
                var obj = context.PopValue();

                context.PushValue(obj.Type.ToString());
            });
            context.RegisterCommand("is_null", (func, context) => { context.PushValue(context.PopValue().Type == SmolType.Null); });
            context.RegisterCommand("is_string", (func, context) => { context.PushValue(context.PopValue().Type == SmolType.String); });
            context.RegisterCommand("is_lambda", (func, context) => { context.PushValue(context.PopValue().Type == SmolType.Lambda); });
            context.RegisterCommand("is_number", (func, context) => { context.PushValue(context.PopValue().Type == SmolType.Number); });
            context.RegisterCommand("is_boolean", (func, context) => { context.PushValue(context.PopValue().Type == SmolType.Boolean); });
            context.RegisterCommand("is_object", (func, context) => { context.PushValue(context.PopValue().Type == SmolType.Object); });

            context.RegisterCommand("invert", (func, context) => { context.PushValue(!context.PopValue().AsBoolean()); });

            context.RegisterCommand("object_keys", (func, context) => {
                var obj = context.PopValue().AsObject();

                context.PushValue(obj.Data.Keys.Select(x => new SmolString(x)).ToArray());
            });

            context.RegisterCommand("object_get", (func, context) => {
                var key = context.PopValue().AsString();
                var obj = context.PopValue().AsObject();

                context.PushValue(obj.Get(key));
            });

            context.RegisterCommand("object_set", (func, context) => {
                var key = context.PopValue().AsString();
                var value = context.PopValue();
                var obj = context.PopValue().AsObject();

                obj.Set(key, value);
            });

            // ======================================== //
            // Working directory and navigation
            // ======================================== //
            // This probably shouldn't be in the system library but more like in the CLI library.
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
            context.RegisterCommand("call", (func, context) => {
                var lambda = context.Pop().AsLambda();

                context.Execute(lambda);
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

            context.RegisterCommand("smol_lex", (func, context) => {
                var input = context.Pop().AsString();

                Lexer lexer = new Lexer(input);

                context.PushValue(lexer.Lex().Select(token => new SmolString(token.Data)).ToArray());
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

            context.RegisterCommand("smol_init", (func, context) => {
                if (!File.Exists("init.smol"))
                {
                    Console.WriteLine("'init.smol' doesn't exist.");
                    return;
                }

                var input = File.ReadAllText("init.smol");
                
                Lexer lexer = new Lexer(input);
                Parser parser = new Parser(lexer.Lex());

                context.Execute(new UnscopedBlockExpression(parser.Parse().ToArray()));
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
