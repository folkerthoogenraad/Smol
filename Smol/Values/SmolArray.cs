using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Values
{
    public class SmolArray : SmolDataBase<SmolValue[]>
    {
        public SmolArray(SmolValue[] data) : base(data)
        {
        }

        public override SmolType Type => SmolType.Array;
        public override string ToString() => $"[{ string.Join(" ", Data.Select(x => x.ToString())) }]";
    }
}
