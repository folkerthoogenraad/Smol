using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Values
{
    public class SmolBoolean : SmolDataBase<bool>
    {
        public SmolBoolean(bool data) : base(data)
        {
        }

        public override SmolType Type => SmolType.Boolean;
        public override string ToString() => $"{Data.ToString().ToLower()}";
    }
}
