using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Expressions
{

    public class LookupSmolExpression : SmolExpression
    {
        public string Name { get; private set; }

        public LookupSmolExpression(string name)
        {
            Name = name;
        }

        public override void Execute(SmolContext context)
        {
            var obj = context.Pop().AsObject();

            context.PushValue(obj.Get(Name));
        }

        public override string ToString()
        {
            return $".{Name}";
        }
    }

}
