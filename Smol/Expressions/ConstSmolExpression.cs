using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Expressions
{
    public class ConstSmolExpression : SmolExpression
    {
        public SmolValue Value { get; set; }

        public ConstSmolExpression(SmolValue value)
        {
            Value = value;
        }

        public override void Execute(SmolContext runtime)
        {
            runtime.PushValue(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}
