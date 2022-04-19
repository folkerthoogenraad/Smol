using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public abstract class Command
    {
        public abstract DataType[] Parameters { get; }
        public abstract DataType[] ReturnTypes { get; }

        public abstract void Execute(Stack<Data> stack);
    }
}
