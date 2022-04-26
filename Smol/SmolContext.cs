using Smol.Expressions;
using Smol.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol
{
    public class SmolContext
    {
        public delegate void SmolProcessor(FunctionCallSmolExpression func, SmolContext context);

        private SmolContext? _parentContext;
        private Stack<SmolValue> _stack;

        private Dictionary<string, SmolProcessor> _actions;
        private SmolObject _this;

        public IEnumerable<SmolValue> Stack => _stack;
        public SmolObject This => _this;

        public SmolContext(SmolContext? parentContext = null, SmolObject? _thisPointer = null)
        {
            _parentContext = parentContext;

            _stack = new Stack<SmolValue>();
            _actions = new Dictionary<string, SmolProcessor>();

            if(_thisPointer == null)
            {
                _this = new SmolObject();
            }
            else
            {
                _this = _thisPointer;
            }
        }

        public void RegisterCommand(string command, SmolProcessor action)
        {
            _actions.Add(command, action);
        }

        public SmolProcessor? FindProcessor(string command)
        {
            if (_actions.ContainsKey(command)) return _actions[command];

            return _parentContext?.FindProcessor(command);
        }

        public SmolValue? GetVariable(string name)
        {
            if (_this.Has(name)) return _this.Get(name);

            return _parentContext?.GetVariable(name);
        }
        public void SetVariable(string name, SmolValue value)
        {
            _this.Set(name, value);
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
        public void PushValue(bool value)
        {
            _stack.Push(new SmolBoolean(value));
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
            _this.Clear();
        }

        public SmolValue Pop()
        {
            return _stack.Pop();
        }

        public SmolValue? ExecuteScoped(SmolExpression command, params SmolValue[] parameters)
        {
            return ExecuteScopedOn(command, null, parameters);
        }

        public SmolValue? ExecuteScopedOn(SmolExpression command, SmolObject? @this, params SmolValue[] parameters)
        {
            SmolContext child = new SmolContext(this, @this);

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
