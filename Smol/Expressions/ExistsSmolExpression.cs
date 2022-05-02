using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Expressions
{
    public class ExistsSmolExpression : SmolExpression
    {
        public SmolExpression InnerExpression { get; set; }

        public ExistsSmolExpression(SmolExpression inner)
        {
            InnerExpression = inner;
        }

        public override void Execute(SmolContext context)
        {
            // This is not at all nice :)
            try
            {
                context.Execute(InnerExpression);
                context.PushValue(true);
            }
            catch
            {
                context.PushValue(false);
            }
        }

        public override string ToString()
        {
            return $"{InnerExpression}?";
        }
    }

}
