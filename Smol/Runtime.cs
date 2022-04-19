using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class Runtime
    {
        public delegate void SmolProcessor(FunctionCallSmolExpression func, Runtime runtime);

        private Runtime _parentRuntime;
        private Stack<SmolValue> _stack;

        private Dictionary<string, SmolProcessor> _actions;
        private Dictionary<string, SmolValue> _variables;

        public IEnumerable<SmolValue> Stack => _stack;
        public IEnumerable<KeyValuePair<string, SmolValue>> DeclaredVariables => _variables;

        public Runtime(Runtime parentRuntime = null)
        {
            _parentRuntime = parentRuntime;

            _stack = new Stack<SmolValue>();
            _actions = new Dictionary<string, SmolProcessor>();
            _variables = new Dictionary<string, SmolValue>();
        }

        public void RegisterCommand(string command, SmolProcessor action)
        {
            _actions.Add(command, action);
        }

        public SmolProcessor? FindProcessor(string command)
        {
            if (_actions.ContainsKey(command)) return _actions[command];

            return _parentRuntime?.FindProcessor(command);
        }

        public SmolValue? GetVariable(string name)
        {
            if (_variables.TryGetValue(name, out SmolValue? value)) return value;

            return _parentRuntime?.GetVariable(name);
        }
        public void SetVariable(string name, SmolValue value)
        {
            _variables[name] = value;
        }

        public SmolValue PopValue()
        {
            return _stack.Pop();
        }

        public void PushValue(string value)
        {
            _stack.Push(new SmolString(value));
        }
        public void PushValue(double value)
        {
            _stack.Push(new SmolNumber(value));
        }
        public void PushValue(SmolValue value)
        {
            _stack.Push(value);
        }
        public void PushValue(SmolValue[] value)
        {
            _stack.Push(new SmolArray(value));
        }

        public int StackHeight()
        {
            return _stack.Count;
        }

        public void ClearStack()
        {
            _stack.Clear();
        }
        public void ClearVariables()
        {
            _variables.Clear();
        }

        public SmolValue Pop()
        {
            return _stack.Pop();
        }

        public SmolValue? ExecuteScoped(SmolExpression command, params SmolValue[] parameters)
        {
            Runtime child = new Runtime(this);

            // Setup the stack
            foreach(var value in parameters) child.PushValue(value);

            child.Execute(command);

            if (child.StackHeight() > 1) throw new ApplicationException("Scoped execution cannot have more than one result.");
            if (child.StackHeight() == 0) return null;

            return child.Pop();
        }

        public void Execute(SmolExpression command)
        {
            command.Execute(this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var s in _stack)
            {
                builder.Append(s);
                builder.Append(' ');
            }

            return builder.ToString();
        }
    }

}
