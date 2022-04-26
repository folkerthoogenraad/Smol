using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Values
{
    public class SmolNumber : SmolDataBase<double>
    {
        public SmolNumber(double data) : base(data)
        {
        }

        public override SmolType Type => SmolType.Number;
        public override string ToString() => FormattableString.Invariant($"{Data}");
    }
}
