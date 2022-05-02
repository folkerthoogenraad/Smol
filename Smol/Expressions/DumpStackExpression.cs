using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Expressions
{
    public class DumpStackExpression : SmolExpression
    {
        public DumpStackExpression()
        {

        }

        public override void Execute(SmolContext context)
        {
            context.ClearStack();
        }

        public override string ToString()
        {
            return ";";
        }
    }

}
