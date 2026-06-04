using _Scripts.Data;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using Utils;

namespace _Scripts.Core.Tools
{
    public abstract class BaseToolCommand : IToolCommand
    {
        protected int _useLeft;
        public int UseLeft => _useLeft;
        protected abstract ToolType ToolType { get; }

        protected BaseToolCommand(int useLeft)
        {
            _useLeft = useLeft;
        }

        public abstract void Execute();

        public abstract bool CanExecute();

        public virtual void Undo()
        {
        }

        public virtual void AddingUses(int amount)
        {
            _useLeft += amount;
            EventBus.Publish(new ToolUseChangeEvent(ToolType, _useLeft));
        }

        protected void ComsumeUse()
        {
            if (_useLeft > 0)
            {
                _useLeft--;
              EventBus.Publish(new ToolUseChangeEvent(ToolType, _useLeft));
            }
        }
    }
}