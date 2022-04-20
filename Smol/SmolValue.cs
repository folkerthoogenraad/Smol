using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public enum SmolType
    {
        String,
        Number,
        Boolean,

        Object,
        Array,

        Lambda
    }

    public abstract class SmolValue
    {
        public abstract SmolType Type { get; }

        public bool IsString => Type == SmolType.String;
        public bool IsNumber => Type == SmolType.Number;
        public bool IsObject => Type == SmolType.Object;
        public bool IsArray => Type == SmolType.Array;
        public bool IsLambda => Type == SmolType.Lambda;
        public bool IsBoolean => Type == SmolType.Boolean;

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

        public virtual string ConvertString()
        {
            return ToString();
        }
    }
    public abstract class SmolBase<T> : SmolValue
    {
        public T Data;

        public SmolBase(T data)
        {
            Data = data;
        }
    }

    public class SmolObject : SmolValue
    {
        public Dictionary<string, SmolValue> Data;

        public SmolObject()
        {
            Data = new Dictionary<string, SmolValue>();
        }

        public bool Has(string name)
        {
            return Data.ContainsKey(name);
        }

        public void Clear()
        {
            Data.Clear();
        }

        public SmolValue Get(string name)
        {
            return Data[name];
        }
        public void Set(string name, SmolValue value)
        {
            Data[name] = value;
        }

        public override SmolType Type => SmolType.Object;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("(");

            foreach (var d in Data)
            {
                sb.Append(d.Value);
                sb.Append(" -> $");
                sb.Append(d.Key);
                sb.Append(" ");
            }

            sb.Append("this)");

            return sb.ToString();
        }

        // TODO this should just be a seperate class/module/whatever
        private void AddIndentation(StringBuilder builder, int indentation)
        {
            while(indentation > 0)
            {
                builder.Append("  ");

                indentation--;
            }
        }

        private string ToFormattedString(int indentation, SmolValue value)
        {

            if (value.IsObject)
            {
                return value.AsObject().ToFormattedString(indentation);
            }
            else
            {
                StringBuilder builder = new StringBuilder();

                AddIndentation(builder, indentation);
                builder.Append(value.ToString());

                return builder.ToString();
            }

        }

        public string ToFormattedString(int indentation = 0) // TODO formatter settigns or something
        {
            StringBuilder sb = new StringBuilder();

            AddIndentation(sb, indentation);
            sb.AppendLine("( ");

            foreach(var var in Data)
            {
                sb.Append(ToFormattedString(indentation + 1, var.Value));
                sb.Append(" -> $");
                sb.AppendLine(var.Key);
            }

            AddIndentation(sb, indentation);
            sb.Append("this)");

            return sb.ToString();
        }
    }

    public class SmolLambda : SmolBase<SmolExpression>
    {
        public SmolLambda(SmolExpression data) : base(data)
        {
        }

        public override SmolType Type => SmolType.Lambda;
        public override string ToString() => $"{{{Data}}}";
    }

    public class SmolNumber : SmolBase<double>
    {
        public SmolNumber(double data) : base(data)
        {
        }

        public override SmolType Type => SmolType.Number;
        public override string ToString() => FormattableString.Invariant($"{Data}");
    }
    public class SmolBoolean : SmolBase<bool>
    {
        public SmolBoolean(bool data) : base(data)
        {
        }

        public override SmolType Type => SmolType.Boolean;
        public override string ToString() => $"{Data.ToString().ToLower()}";
    }
    public class SmolString : SmolBase<string>
    {
        public SmolString(string data) : base(data)
        {
        }

        public override SmolType Type => SmolType.String;
        public override string ToString() => $"\"{Data.Replace("\n", "\\n")}\"";
        public override string ConvertString() => $"{Data}";
    }
    public class SmolArray : SmolBase<SmolValue[]>
    {
        public SmolArray(SmolValue[] data) : base(data)
        {
        }

        public override SmolType Type => SmolType.Array;
        public override string ToString() => $"[{ string.Join(" ", Data.Select(x => x.ToString())) }]";
    }
}
