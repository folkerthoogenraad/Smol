using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Smol.Values
{
    // Maybe make this smol optional
    public class SmolNull : SmolValue
    {
        public SmolNull()
        {
        }

        public override SmolType Type => SmolType.Null;
        public override string ToString() => "null";
        public override string ConvertString() => "null";
    }
}
