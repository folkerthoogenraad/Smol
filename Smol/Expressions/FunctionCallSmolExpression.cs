using Smol.Values;
using System.Text;

namespace Smol.Expressions
{
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

        public string GetString(SmolContext context, string name)
        {
            var value = GetValue(context, name);

            if (value is SmolString str) return str.Data;

            throw new ApplicationException($"Cannot get value {name} as string.");
        }

        public double GetNumber(SmolContext context, string name)
        {
            var value = GetValue(context, name);

            if (value is SmolNumber num) return num.Data;

            throw new ApplicationException($"Cannot get value {name} as number.");
        }

        public SmolValue GetValue(SmolContext context, string name)
        {
            if (!Parameters.TryGetValue(name, out var command))
            {
                throw new ApplicationException($"Cannot find parameter with name: {name}");
            }

            // Create a seperate runtime for this
            SmolContext runtime = new SmolContext(context, context.This);

            command.Execute(runtime);

            return runtime.PopValue();
        }

        public override void Execute(SmolContext context)
        {
            var processor = context.FindProcessor(Name);

            if (processor == null)
            {
                throw new Exception($"Unknown function with name \"{Name}\"!");
            }

            processor.Invoke(this, context);
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
