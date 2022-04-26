using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Modules
{
    public class Maths
    {
        public static void Initialize(SmolContext context)
        {
            // ============================================= //
            // Basic math operations
            // ============================================= //
            context.RegisterCommand("add", (func, context) => {
                var num2 = context.Pop().AsNumber();
                var num1 = context.Pop().AsNumber();

                context.PushValue(num1 + num2);
            });
            context.RegisterCommand("sub", (func, context) => {
                var num2 = context.Pop().AsNumber();
                var num1 = context.Pop().AsNumber();

                context.PushValue(num1 - num2);
            });
            context.RegisterCommand("mul", (func, context) => {
                var num2 = context.Pop().AsNumber();
                var num1 = context.Pop().AsNumber();

                context.PushValue(num1 * num2);
            });
            context.RegisterCommand("div", (func, context) => {
                var num2 = context.Pop().AsNumber();
                var num1 = context.Pop().AsNumber();

                context.PushValue(num1 / num2);
            });
        }
    }
}
