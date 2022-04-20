using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public static class Arrays
    {
        public static void Initialize(Runtime runtime)
        {
            runtime.RegisterCommand("each", (func, runtime) =>
            {
                var lambda = runtime.Pop().AsLambda();
                var array = runtime.Pop().AsArray();

                runtime.PushValue(array
                    .Select(x => runtime.ExecuteScoped(lambda, x))
                    .Where(x => x != null)
                    .ToArray());
            });

            runtime.RegisterCommand("filter", (func, runtime) => {
                var lambda = runtime.Pop().AsLambda();
                var array = runtime.Pop().AsArray();

                runtime.PushValue(array
                    .Where(x => runtime.ExecuteScoped(lambda, x).AsBoolean())
                    .ToArray());
            });

            runtime.RegisterCommand("reverse", (func, runtime) => {
                var lambda = runtime.Pop().AsLambda();
                var array = runtime.Pop().AsArray();

                runtime.PushValue(array
                    .Reverse()
                    .ToArray());
            });

            runtime.RegisterCommand("order", (func, runtime) => {
                var lambda = runtime.Pop().AsLambda();
                var array = runtime.Pop().AsArray();

                runtime.PushValue(array
                    .OrderBy(x => runtime.ExecuteScoped(lambda, x).AsNumber())
                    .ToArray());
            });

            runtime.RegisterCommand("range", (func, runtime) => {
                var array = runtime.Pop().AsArray();

                int from = 0;
                int to = array.Length - 1;

                if (func.HasParameter("from"))
                {
                    from = (int)func.GetNumber(runtime, "from");
                }
                if (func.HasParameter("to"))
                {
                    to = (int)func.GetNumber(runtime, "to");
                }

                if (to > array.Length)
                {
                    throw new ApplicationException("Index out of range.");
                }
                if (from < 0)
                {
                    throw new ApplicationException("Index out of range.");
                }
                if (from >= to)
                {
                    throw new ApplicationException("from > to");
                }

                int length = to - from;

                var newArray = new SmolValue[length];

                Array.Copy(array, from, newArray, 0, length);

                runtime.PushValue(newArray);
            });

            runtime.RegisterCommand("first", (func, runtime) => {
                var array = runtime.Pop().AsArray();

                runtime.PushValue(array[0]);
            });
            
            runtime.RegisterCommand("second", (func, runtime) => {
                var array = runtime.Pop().AsArray();

                runtime.PushValue(array[1]);
            });
        }
    }
}
