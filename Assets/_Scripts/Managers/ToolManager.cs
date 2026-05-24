using System;
using System.Collections.Generic;
using _Scripts.Core.Tools;
using _Scripts.Data;
using Grid_Map;
using UnityEngine;
using Utils;

namespace _Scripts.Managers
{
    public class ToolManager: Singleton<ToolManager>
    {
        [Header("Invoker & Receiver")]
        [SerializeField] private ToolData toolData;

        private GridSpawner _gridSpawner;
        private ShellManager _shellManager;
        private readonly Dictionary<ToolType, IToolCommand> _commands = new Dictionary<ToolType, IToolCommand>();
        public void Initialize(GridSpawner gridSpawner, ShellManager shellManager)
        {
            _gridSpawner = gridSpawner;
            _shellManager = shellManager;
            InitCommands();
        }
        private void InitCommands()
        {
            _commands.Clear();
          _commands[ToolType.Shuffle] = new ShuffleTool(_gridSpawner,toolData.shuffleUseInALevel);
            _commands[ToolType.AddSlot] = new AddSlotTool(_shellManager,toolData.addSlotUseInALevel);
            _commands[ToolType.Hint] = new HintTool();
          _commands[ToolType.Return] = new ReturnTool(_shellManager,3);
        }

        public void UseShuffle() => Execute(ToolType.Shuffle);
        public void UseAddSlot()=> Execute(ToolType.AddSlot);
        public void UseHint()=> Execute(ToolType.Hint);
        public void UseReturn() => Execute(ToolType.Return);

        private void Execute(ToolType toolType)
        {
            if (!_commands.TryGetValue(toolType,out var command)) return;

            if (!command.CanExecute())
            {
                TileEventBus.OnToolOutOfUse?.Invoke(toolType);
                 return;
            }
            command.Execute();
            
        }

        public void ResetForNewLevel() => InitCommands();

        public int GetUseLeft(ToolType toolType) =>
            _commands.TryGetValue(toolType, out var command) ? command.UseLeft : 0;

    }

}