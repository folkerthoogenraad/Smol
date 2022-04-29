using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Attributes
{
    public class FromParameters : Attribute
    {
        public string Name { get; set; }

        public FromParameters(string name)
        {
            Name = name;
        }
    }
}
