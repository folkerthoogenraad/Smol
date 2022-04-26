using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Values
{
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
            while (indentation > 0)
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

            foreach (var var in Data)
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
}
