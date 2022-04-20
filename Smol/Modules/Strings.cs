using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public class Strings
    {
        public static void Initialize(Runtime runtime)
        {
            runtime.RegisterCommand("split", (func, runtime) => {
                var value = runtime.Pop().AsString();

                string on = "";

                if (func.HasParameter("on"))
                {
                    on = func.GetString(runtime, "on");
                }

                var splitted = value.Split(on);

                runtime.PushValue(splitted.Select(x => new SmolString(x)).ToArray());
            });
            runtime.RegisterCommand("trim", (func, runtime) => {
                var value = runtime.Pop().AsString();

                runtime.PushValue(value.Trim());
            });
            runtime.RegisterCommand("equals", (func, runtime) => {
                var valuea = runtime.Pop().AsString();
                var valueb = runtime.Pop().AsString();

                runtime.PushValue(valuea == valueb);
            });
            runtime.RegisterCommand("contains", (func, runtime) => {
                var search = runtime.Pop().AsString();
                var container = runtime.Pop().AsString();

                runtime.PushValue(container.Contains(search));
            });
            runtime.RegisterCommand("append", (func, runtime) => {
                var to = runtime.Pop().AsString();
                var from = runtime.Pop().AsString();

                runtime.PushValue(from + to);
            });
            runtime.RegisterCommand("join", (func, runtime) => {
                var array = runtime.Pop().AsArray();

                string with = string.Empty;

                if (func.HasParameter("with"))
                {
                    with = func.GetString(runtime, "with");
                }

                runtime.PushValue(string.Join(with, array.Select(x => x.ConvertString())));
            });
        }
    }
}
