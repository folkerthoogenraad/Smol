using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class LambdaCommand : Command
    {
        public override DataType[] Parameters => new DataType[0];
        public override DataType[] ReturnTypes => new DataType[0];

        public Action<Stack<Data>> Action;

        public LambdaCommand(Action<Stack<Data>> action)
        {
            Action = action;
        }

        public override void Execute(Stack<Data> stack)
        {
            Action(stack);
        }
    }

    public static class RuntimeExtensions
    {
        public static void RegisterCommand(this Runtime runtime, string command, Action<Stack<Data>> action)
        {
            runtime.RegisterCommand(command, new LambdaCommand(action));
        }
    }
}
