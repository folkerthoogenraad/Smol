using Smol.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Values
{
    public class SmolLambda : SmolDataBase<SmolExpression>
    {
        public SmolLambda(SmolExpression data) : base(data)
        {
        }

        public override SmolType Type => SmolType.Lambda;
        public override string ToString() => $"{{{Data}}}";
    }
}
