using _Scripts.Core.Tile;
using _Scripts.Data;
using _Scripts.Data.Tool;
using _Scripts.Managers;

namespace _Scripts.Core.Tools
{
    public class AddSlotTool : BaseToolCommand
    {
        private readonly ShellManager _shellManager;
        protected override ToolType ToolType => ToolType.AddSlot;

        public AddSlotTool(ShellManager shellManager, int useLeft, int useLeftInALevel) : base(useLeft, useLeftInALevel)
        {
            _shellManager = shellManager;
        }

        public override void Execute()
        {
            if (!CanExecute()) return;

            _shellManager.AddSlot();

            ComsumeUse();
        }

        public override bool CanExecute() => _shellManager != null && _useLeft > 0 && _useLeftInALevel > 0;
    }
}