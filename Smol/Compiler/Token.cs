using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Compiler
{
    public class Token
    {
        public enum TokenType
        {
            Identifier,
            Equals,

            Number,
            String,
            Boolean,
            Null,

            Param,

            Seperator,

            Question,

            BracketOpen,
            BracketClose,

            ArrayOpen,
            ArrayClose,

            CurlyOpen,
            CurlyClose,

            LineEnd,

            Lookup,

            Variable,
            Store,

            EndOfFile
        }

        public TokenType Type { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: ({1})", Type, Data);
        }
    }
}
