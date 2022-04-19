using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class Runtime
    {
        private Dictionary<string, Command> _commands;
        private Stack<Data> _stack;

        public Runtime()
        {
            _commands = new Dictionary<string, Command>();
            _stack = new Stack<Data>();
        }

        public void RegisterCommand(string name, Command cmd)
        {
            _commands[name] = cmd;
        }

        // TODO this shouldn't take tokens, but should take something from the parser but this is good enough for now
        public void Execute(IEnumerable<Token> tokens)
        {
            foreach(var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.String:
                        _stack.Push(new StringData(token.Data));
                        break;
                    case TokenType.Number:
                        _stack.Push(new NumberData(double.Parse(token.Data)));
                        break;
                    case TokenType.Identifier:
                        if(!_commands.TryGetValue(token.Data, out var command)) {
                            Console.WriteLine("Nooo!");
                            return;
                        }
                        command.Execute(_stack);
                        break;
                }
            }
        }
    }
}
