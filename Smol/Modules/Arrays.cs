using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public static class Arrays
    {
        public static void Initialize(SmolContext context)
        {
            context.RegisterCommand("map", (func, context) =>
            {
                var lambda = context.Pop().AsLambda();
                var array = context.Pop().AsArray();

                context.PushValue(array
                    .Select(x => context.ExecuteScoped(lambda, x))
                    .Where(x => x != null)
                    .ToArray());
            });

            context.RegisterCommand("each", (func, context) =>
            {
                var lambda = context.Pop().AsLambda();
                var array = context.Pop().AsArray();

                foreach(var value in array)
                {
                    context.ExecuteScopedOn(lambda, value);
                }
            });

            context.RegisterCommand("filter", (func, context) => {
                var lambda = context.Pop().AsLambda();
                var array = context.Pop().AsArray();

                context.PushValue(array
                    .Where(x => context.ExecuteScoped(lambda, x).AsBoolean())
                    .ToArray());
            });

            context.RegisterCommand("reverse", (func, context) => {
                var lambda = context.Pop().AsLambda();
                var array = context.Pop().AsArray();

                context.PushValue(array
                    .Reverse()
                    .ToArray());
            });

            context.RegisterCommand("order", (func, context) => {
                var lambda = context.Pop().AsLambda();
                var array = context.Pop().AsArray();

                context.PushValue(array
                    .OrderBy(x => context.ExecuteScoped(lambda, x).AsNumber())
                    .ToArray());
            });

            context.RegisterCommand("range", (func, context) => {
                var array = context.Pop().AsArray();

                int from = 0;
                int to = array.Length - 1;

                if (func.HasParameter("from"))
                {
                    from = (int)func.GetNumber(context, "from");
                }
                if (func.HasParameter("to"))
                {
                    to = (int)func.GetNumber(context, "to");
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

                context.PushValue(newArray);
            });

            context.RegisterCommand("first", (func, context) => {
                var array = context.Pop().AsArray();

                context.PushValue(array[0]);
            });
            
            context.RegisterCommand("second", (func, context) => {
                var array = context.Pop().AsArray();

                context.PushValue(array[1]);
            });
        }
    }
}
