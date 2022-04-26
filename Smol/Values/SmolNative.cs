using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Values
{
    public class SmolNative : SmolDataBase<object>
    {
        public SmolNative(object data) : base(data)
        {
        }

        public override SmolType Type => SmolType.Native;
        public override string? ToString() => Data?.ToString();
        public override string ConvertString() => $"{Data}";
    }
}
