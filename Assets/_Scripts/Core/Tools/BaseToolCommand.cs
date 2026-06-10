using _Scripts.Data;
using _Scripts.Data.Tool;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;

namespace _Scripts.Core.Tools
{
    public abstract class BaseToolCommand : IToolCommand
    {
        protected int _useLeft;
        protected int _useLeftInALevel;
        public int UseLeft => _useLeft;
        protected abstract ToolType ToolType { get; }

        protected BaseToolCommand(int useLeft,int  useLeftInALevel)
        {
            _useLeft = useLeft;
            _useLeftInALevel = useLeftInALevel;
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
                _useLeftInALevel--;
              EventBus.Publish(new ToolUseChangeEvent(ToolType, _useLeft));
            }
        }
    }
}