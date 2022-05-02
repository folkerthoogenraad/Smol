using Smol.Expressions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Values
{
    public abstract class SmolValue
    {
        public abstract SmolType Type { get; }

        public bool IsString => Type == SmolType.String;
        public bool IsNumber => Type == SmolType.Number;
        public bool IsObject => Type == SmolType.Object;
        public bool IsArray => Type == SmolType.Array;
        public bool IsLambda => Type == SmolType.Lambda;
        public bool IsBoolean => Type == SmolType.Boolean;
        public bool IsNative => Type == SmolType.Native;
        public bool IsNull => Type == SmolType.Null;

        public string AsString()
        {
            if (this is SmolString str) return str.Data;

            throw new ArgumentException("Not a string");
        }
        public double AsNumber()
        {
            if (this is SmolNumber num) return num.Data;

            throw new ArgumentException("Not a number");
        }
        public SmolValue[] AsArray()
        {
            if (this is SmolArray array) return array.Data;

            throw new ArgumentException("Not an array");
        }
        public SmolExpression AsLambda()
        {
            if (this is SmolLambda array) return array.Data;

            throw new ArgumentException("Not a lambda");
        }
        public SmolObject AsObject()
        {
            if (this is SmolObject obj) return obj;

            throw new ArgumentException("Not an object");
        }
        public bool AsBoolean()
        {
            if (this is SmolBoolean boo) return boo.Data;

            throw new ArgumentException("Not a boolean");
        }
        public object AsNative()
        {
            if (this is SmolBoolean boo) return boo.Data;

            throw new ArgumentException("Not a boolean");
        }

        public virtual string ConvertString()
        {
            return ToString();
        }
    }
    

}
