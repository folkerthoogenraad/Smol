using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Expressions
{
    public class VariableSmolExpression : SmolExpression
    {
        public string Name { get; private set; }

        public VariableSmolExpression(string name)
        {
            Name = name;
        }

        public override void Execute(SmolContext context)
        {
            var value = context.GetVariable(Name);

            if (value == null)
            {
                throw new Exception($"Unknown variable ${Name}");
            }

            context.PushValue(value);
        }

        public override string ToString()
        {
            return $"${Name}";
        }
    }
}
