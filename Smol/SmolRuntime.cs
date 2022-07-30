using Smol.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class SmolRuntime
    {
        private SmolContext _context;

        public SmolContext Context => _context;

        public SmolRuntime()
        {
            SmolContext context = new SmolContext();

            Modules.System.Initialize(context);
            Modules.IO.Initialize(context);
            Modules.Arrays.Initialize(context);
            Modules.Maths.Initialize(context);
            Modules.Strings.Initialize(context);
            Modules.JSON.Initialize(context);

            _context = context;
        }

        public void Excecute(string smol)
        {
            var tokens = Lexer.Lex(smol);
            var commands = Parser.Parse(tokens);

            _context.Execute(commands);
            _context.ClearStack();
        }
    }
}
