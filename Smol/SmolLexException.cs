using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class SmolLexException : SmolException
    {
        public SmolLexException(string[] messages) : base(messages) { }
    }
}
