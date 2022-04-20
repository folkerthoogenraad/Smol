using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public class Maths
    {
        public static void Initialize(Runtime runtime)
        {
            // ============================================= //
            // Basic math operations
            // ============================================= //
            runtime.RegisterCommand("add", (func, runtime) => {
                var num2 = runtime.Pop().AsNumber();
                var num1 = runtime.Pop().AsNumber();

                runtime.PushValue(num1 + num2);
            });
            runtime.RegisterCommand("sub", (func, runtime) => {
                var num2 = runtime.Pop().AsNumber();
                var num1 = runtime.Pop().AsNumber();

                runtime.PushValue(num1 - num2);
            });
            runtime.RegisterCommand("mul", (func, runtime) => {
                var num2 = runtime.Pop().AsNumber();
                var num1 = runtime.Pop().AsNumber();

                runtime.PushValue(num1 * num2);
            });
            runtime.RegisterCommand("div", (func, runtime) => {
                var num2 = runtime.Pop().AsNumber();
                var num1 = runtime.Pop().AsNumber();

                runtime.PushValue(num1 / num2);
            });
        }
    }
}
