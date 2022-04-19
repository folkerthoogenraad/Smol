using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Compiler
{
    public class Lexer
    {
        private Iterator<char> _characters;

        public Lexer(string data)
        {
            _characters = new Iterator<char>(data.ToCharArray());
        }

        public IEnumerable<Token> Lex()
        {
            while (_characters.HasCurrent)
            {
                char c = _characters.Current();

                // strings
                if (c == '"')
                {
                    c = _characters.Next();

                    StringBuilder builder = new StringBuilder();

                    while (_characters.HasCurrent && c != '"')
                    {
                        builder.Append(c);

                        c = _characters.Next();
                    }

                    if (_characters.HasCurrent) _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.String,
                        Data = builder.ToString().Replace("\\n", "\n")
                    };
                }
                else if (c == '-')
                {
                    c = _characters.Next();

                    // Arrow
                    if(c == '>')
                    {
                        yield return new Token()
                        {
                            Type = Token.TokenType.Store,
                            Data = "->"
                        };
                    }

                    // Param
                    else
                    {
                        StringBuilder builder = new StringBuilder();

                        while (_characters.HasCurrent && IsCharacter(c))
                        {
                            builder.Append(c);

                            c = _characters.Next();
                        }

                        yield return new Token()
                        {
                            Type = Token.TokenType.Param,
                            Data = builder.ToString()
                        };

                    }

                }
                else if (c == '.')
                {
                    c = _characters.Next();

                    StringBuilder builder = new StringBuilder();

                    while (_characters.HasCurrent && IsCharacter(c))
                    {
                        builder.Append(c);

                        c = _characters.Next();
                    }

                    yield return new Token()
                    {
                        Type = Token.TokenType.Lookup,
                        Data = builder.ToString()
                    };
                }
                else if (c == '$')
                {
                    c = _characters.Next();

                    StringBuilder builder = new StringBuilder();

                    while (_characters.HasCurrent && IsCharacter(c))
                    {
                        builder.Append(c);

                        c = _characters.Next();
                    }

                    yield return new Token()
                    {
                        Type = Token.TokenType.Variable,
                        Data = builder.ToString()
                    };
                }
                else if (c == '=')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.Equals,
                        Data = "="
                    };
                }
                else if (c == ',')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.Seperator,
                        Data = ","
                    };
                }
                else if (c == '(')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.BracketOpen,
                        Data = "("
                    };
                }
                else if (c == ')')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.BracketClose,
                        Data = ")"
                    };
                }

                else if (c == '{')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.CurlyOpen,
                        Data = "{"
                    };
                }
                else if (c == '}')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.CurlyClose,
                        Data = "}"
                    };
                }
                else if (c == '[')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.ArrayOpen,
                        Data = "["
                    };
                }
                else if (c == ']')
                {
                    _characters.Next();

                    yield return new Token()
                    {
                        Type = Token.TokenType.ArrayClose,
                        Data = "]"
                    };
                }
                else if (IsCharacter(c))
                {
                    StringBuilder builder = new StringBuilder();

                    while (_characters.HasCurrent && (IsCharacter(c) || IsNumber(c)))
                    {
                        builder.Append(c);

                        c = _characters.Next();
                    }

                    yield return new Token()
                    {
                        Type = Token.TokenType.Identifier,
                        Data = builder.ToString()
                    };
                }
                else if (IsNumber(c))
                {
                    StringBuilder builder = new StringBuilder();

                    while (_characters.HasCurrent && (IsNumber(c) || c == '.'))
                    {
                        builder.Append(c);

                        c = _characters.Next();
                    }

                    yield return new Token()
                    {
                        Type = Token.TokenType.Number,
                        Data = builder.ToString()
                    };
                }
                // Comments!
                else if(c == '#')
                {
                    while(_characters.HasCurrent && c != '\n')
                    {
                        c = _characters.Next();
                    }
                }
                else
                {
                    _characters.Next();
                }
            }

            yield return new Token()
            {
                Type = Token.TokenType.EndOfFile,
                Data = ""
            };
        }

        public bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }
        public bool IsCharacter(char c)
        {
            return (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z')
                || c == '_'
                || IsNumber(c);
        }
        public bool IsNumber(char c)
        {
            return (c >= '0' && c <= '9');
        }
    }

}
