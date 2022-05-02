using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Expressions
{
    public class ArraySmolExpression : SmolExpression
    {
        public SmolExpression[] Expressions { get; private set; }

        public ArraySmolExpression(SmolExpression[] expressions)
        {
            Expressions = expressions;
        }

        public override void Execute(SmolContext context)
        {
            SmolContext r = new SmolContext(context);

            foreach (var expression in Expressions)
            {
                expression.Execute(r);
            }

            context.PushValue(r.Stack.Reverse().ToArray());
        }

        public override string ToString()
        {
            return $"[{string.Join(" ", Expressions.Select(x => x.ToString()))}]";
        }
    }


}
