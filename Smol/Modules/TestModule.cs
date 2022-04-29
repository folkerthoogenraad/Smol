using Smol.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public class TestModule
    {
        [Function("print")]
        public void Print([FromStack] string content)
        {
            Console.WriteLine(content);
        }

    }
}
