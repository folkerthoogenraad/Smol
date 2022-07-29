using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Expressions
{
    public class ParentStackExpression : SmolExpression
    {
        public ParentStackExpression()
        {
        }

        public override void Execute(SmolContext context)
        {
            if(context.ParentContext == null)
            {
                return;
            }

            context.PushValue(context.ParentContext.PeekValue());
        }

        public override string ToString()
        {
            return "@";
        }
    }

}
