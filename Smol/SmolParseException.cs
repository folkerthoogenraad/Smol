using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class SmolParseException : SmolException
    {
        public SmolParseException(string[] messages) : base(messages) { }
    }
}
