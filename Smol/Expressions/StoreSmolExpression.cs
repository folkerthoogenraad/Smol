using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Expressions
{
    public class StoreSmolExpression : SmolExpression
    {
        public string[] LookupChain { get; private set; }

        public StoreSmolExpression(string[] lookupChain)
        {
            LookupChain = lookupChain;
        }

        public override void Execute(SmolContext context)
        {
            var value = context.PopValue();

            var obj = context.This;

            for (int i = 0; i < LookupChain.Length - 1; i++)
            {
                obj = obj.Get(LookupChain[i]).AsObject();
            }

            obj.Set(LookupChain.Last(), value);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("-> $");
            builder.Append(LookupChain[0]);
            for (int i = 1; i < LookupChain.Length; i++)
            {
                builder.Append(" .");
                builder.Append(LookupChain[i]);
            }

            return builder.ToString();
        }
    }
}
