using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smol.Expressions
{
    public abstract class SmolExpression
    {
        public abstract void Execute(SmolContext context);
    }
}
