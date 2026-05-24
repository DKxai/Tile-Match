using _Scripts.Data;
using Utils;

namespace _Scripts.Core.Tools
{
    public class AddSlotTool : IToolCommand
    {
        private ShellManager _shellManager;
        private int _useLeft;

        public AddSlotTool(ShellManager shellManager, int useLeft)
        {
            _shellManager = shellManager;
            _useLeft = useLeft;
        }
        public int UseLeft => _useLeft;
        public void Execute()
        {
            if (!CanExecute()) return;

            _shellManager.AddSlot();

            if (_useLeft > 0) _useLeft--;
            TileEventBus.OnToolUsed?.Invoke(ToolType.AddSlot,_useLeft);
        }

        public bool CanExecute() => _shellManager != null && _useLeft > 0;
    }
}