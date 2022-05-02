using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Smol.Values
{
    public class SmolString : SmolDataBase<string>
    {
        public SmolString(string data) : base(data)
        {
        }

        public override SmolType Type => SmolType.String;
        public override string ToString() => $"\"{Language.Escape(Data)}\"";
        public override string ConvertString() => $"{Data}";
    }
}
