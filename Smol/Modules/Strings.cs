using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public class Strings
    {
        public static void Initialize(SmolContext context)
        {
            context.RegisterCommand("split", (func, context) => {
                var value = context.Pop().AsString();

                string on = "";

                if (func.HasParameter("on"))
                {
                    on = func.GetString(context, "on");
                }

                var splitted = value.Split(on);

                context.PushValue(splitted.Select(x => new SmolString(x)).ToArray());
            });
            context.RegisterCommand("trim", (func, context) => {
                var value = context.Pop().AsString();

                context.PushValue(value.Trim());
            });
            context.RegisterCommand("equals", (func, context) => {
                var valuea = context.Pop().AsString();
                var valueb = context.Pop().AsString();

                context.PushValue(valuea == valueb);
            });
            context.RegisterCommand("contains", (func, context) => {
                var search = context.Pop().AsString();
                var container = context.Pop().AsString();

                context.PushValue(container.Contains(search));
            });
            context.RegisterCommand("append", (func, context) => {
                var to = context.Pop().AsString();
                var from = context.Pop().AsString();

                context.PushValue(from + to);
            });
            context.RegisterCommand("join", (func, context) => {
                var array = context.Pop().AsArray();

                string with = string.Empty;

                if (func.HasParameter("with"))
                {
                    with = func.GetString(context, "with");
                }

                context.PushValue(string.Join(with, array.Select(x => x.ConvertString())));
            });
        }
    }
}
