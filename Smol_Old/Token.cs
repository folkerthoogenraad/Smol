using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public enum TokenType
    {
        Identifier,
        String,
        Number,

        BracketOpen,
        BracketClose,
    }

    public class Token
    {
        public string Data { get; set; }
        public TokenType Type { get; set; }

        public Token(string data, TokenType type)
        {
            Data = data;
            Type = type;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Type, Data);
        }
    }
}
