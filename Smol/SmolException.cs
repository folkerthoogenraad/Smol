using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class SmolException : Exception
    {
        public string[] Messages { get; set; }

        public SmolException(string[] messages)
        {
            Messages = messages;
        }
    }
}
