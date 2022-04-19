using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class Lexer
    {
        public Lexer()
        {
        }

        public IEnumerable<Token> Lex(IEnumerable<char> characters)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var character in characters)
            {
                if (char.IsWhiteSpace(character))
                {
                    yield return new Token(builder.ToString(), TokenType.Identifier);
                    builder.Clear();
                    continue;
                }
                
                builder.Append(character);
            }

            yield return new Token(builder.ToString(), TokenType.Identifier);
        }


        public static IEnumerable<Token> Tokenize(IEnumerable<char> characters)
        {
            Lexer lexer = new Lexer();

            return lexer.Lex(characters);
        }
    }
}
