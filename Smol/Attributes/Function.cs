using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Attributes
{
    public class Function : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Function(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}
