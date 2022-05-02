using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class Language
    {
        private record struct EscapedCharacters(string Escaped, string Raw);

        private static List<EscapedCharacters> escapedCharacters = new List<EscapedCharacters>() { 
            new EscapedCharacters("\\n", "\n"),
            new EscapedCharacters("\\r", "\r"),
            new EscapedCharacters("\\t", "\t"),
            new EscapedCharacters("\\\"", "\""),
        };

        public static string Escape(string s)
        {
            foreach(var c in escapedCharacters)
            {
                s = s.Replace(c.Raw, c.Escaped);
            }

            return s;
        }

        public static string Unescape(string s)
        {
            foreach (var c in escapedCharacters)
            {
                s = s.Replace(c.Escaped, c.Raw);
            }

            return s;
        }
    }
}
