using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public abstract class Data
    {
        public abstract DataType Type { get; }
        public bool IsNumber => Type == DataType.Number;
        public bool IsString => Type == DataType.String;
        public bool IsObject => Type == DataType.Object;
    }

    public class NumberData : Data
    {
        public override DataType Type => DataType.Number;
        public double Value { get; set; }
        public NumberData(double v)
        {
            Value = v;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
    
    public class StringData : Data
    {
        public override DataType Type => DataType.String;
        public string Value { get; set; }

        public StringData(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return string.Format($"\"{Value}\"");
        }
    }

    public class ObjectData : Data
    {
        public override DataType Type => DataType.Object;
    }
}
