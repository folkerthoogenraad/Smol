using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public abstract class SmolExpression
    {
        public abstract void Execute(Runtime runtime);
    }

    public class ConstSmolExpression : SmolExpression
    {
        public SmolValue Value { get; set; }

        public ConstSmolExpression(SmolValue value)
        {
            Value = value;
        }

        public override void Execute(Runtime runtime)
        {
            runtime.PushValue(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class ArraySmolExpression : SmolExpression
    {
        public SmolExpression[] Expressions { get; private set; }

        public ArraySmolExpression(SmolExpression[] expressions)
        {
            Expressions = expressions;
        }

        public override void Execute(Runtime runtime)
        {
            var values = new List<SmolValue>();

            foreach (var expression in Expressions)
            {
                Runtime r = new Runtime(runtime);

                expression.Execute(r);

                if (r.StackHeight() > 1) throw new ApplicationException("Inner block cannot have a stackheight of more than 1");

                if (r.StackHeight() == 1)
                {
                    values.Add(r.PopValue());
                }
                else
                {
                    throw new ApplicationException("Array must have at least one stack.");
                }
            }

            runtime.PushValue(values.ToArray());
        }

        public override string ToString()
        {
            return $"[{string.Join(" ", Expressions.Select(x => x.ToString()))}]";
        }
    }


    public class LookupSmolExpression : SmolExpression
    {
        public string Name { get; private set; }

        public LookupSmolExpression(string name)
        {
            Name = name;
        }

        public override void Execute(Runtime runtime)
        {
            var obj = runtime.Pop().AsObject();

            runtime.PushValue(obj.Get(Name));
        }

        public override string ToString()
        {
            return $".{Name}";
        }
    }

    public class VariableSmolExpression : SmolExpression
    {
        public string Name { get; private set; }

        public VariableSmolExpression(string name)
        {
            Name = name;
        }

        public override void Execute(Runtime runtime)
        {
            var value = runtime.GetVariable(Name);

            if (value == null)
            {
                throw new Exception($"Unknown variable ${Name}");
            }

            runtime.PushValue(value);
        }

        public override string ToString()
        {
            return $"${Name}";
        }
    }
    public class StoreSmolExpression : SmolExpression
    {
        public string Name { get; private set; }

        public StoreSmolExpression(string name)
        {
            Name = name;
        }

        public override void Execute(Runtime runtime)
        {
            var value = runtime.PopValue();

            runtime.SetVariable(Name, value);
        }

        public override string ToString()
        {
            return $"${Name}";
        }
    }

    public class UnscopedBlockExpression : SmolExpression
    {
        public SmolExpression[] Expressions { get; private set; }

        public UnscopedBlockExpression(SmolExpression[] expressions)
        {
            Expressions = expressions;
        }

        public override void Execute(Runtime runtime)
        {
            foreach (var expression in Expressions)
            {
                runtime.Execute(expression);
            }
        }

        public override string ToString()
        {
            return $"{string.Join(" ", Expressions.Select(x => x.ToString()))}";
        }
    }

    public class BlockSmolExpression : SmolExpression
    {
        public SmolExpression[] Expressions { get; private set; }

        public BlockSmolExpression(SmolExpression[] expressions)
        {
            Expressions = expressions;
        }

        public override void Execute(Runtime runtime)
        {
            Runtime r = new Runtime(runtime);
            
            foreach (var expression in Expressions)
            {
                expression.Execute(r);
            }

            if (r.StackHeight() > 1) throw new ApplicationException("Inner block cannot have a stackheight of more than 1");

            if(r.StackHeight() == 1)
            {
                runtime.PushValue(r.PopValue());
            }
        }

        public override string ToString()
        {
            return $"({string.Join(" ", Expressions.Select(x => x.ToString()))})";
        }
    }

    public class FunctionCallSmolExpression : SmolExpression
    {
        public string Name { get; set; }
        public Dictionary<string, SmolExpression> Parameters { get; set; }

        public FunctionCallSmolExpression(string name)
        {
            Name = name;
            Parameters = new Dictionary<string, SmolExpression>();
        }

        public bool HasParameter(string p)
        {
            return Parameters.ContainsKey(p);
        }

        public string GetString(Runtime runtime, string name)
        {
            var value = GetValue(runtime, name);

            if (value is SmolString str) return str.Data;

            throw new ApplicationException($"Cannot get value {name} as string.");
        }

        public double GetNumber(Runtime runtime, string name)
        {
            var value = GetValue(runtime, name);

            if (value is SmolNumber num) return num.Data;

            throw new ApplicationException($"Cannot get value {name} as number.");
        }

        public SmolValue GetValue(Runtime existingRuntime, string name)
        {
            if (!Parameters.TryGetValue(name, out var command))
            {
                throw new ApplicationException($"Cannot find parameter with name: {name}");
            }

            // Create a seperate runtime for this
            Runtime runtime = new Runtime(existingRuntime);

            command.Execute(runtime);

            return runtime.PopValue();
        }

        public override void Execute(Runtime runtime)
        {
            var processor = runtime.FindProcessor(Name);

            if (processor == null)
            {
                throw new Exception($"Unknown function with name \"{Name}\"!");
            }

            processor.Invoke(this, runtime);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Name);

            foreach(var parameter in Parameters)
            {
                sb.Append(" -");
                sb.Append(parameter.Key);
                sb.Append(" ");
                sb.Append(parameter.Value);
            }

            return sb.ToString();
        }
    }
}
