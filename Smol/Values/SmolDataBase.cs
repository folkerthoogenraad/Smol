using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Values
{
    public abstract class SmolDataBase<T> : SmolValue
    {
        public T Data;

        public SmolDataBase(T data)
        {
            Data = data;
        }
    }

}
