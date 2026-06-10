using System;
using System.Collections.Generic;
using _Scripts.Core.Grid;
using _Scripts.Core.Tile;
using _Scripts.Core.Tools;
using _Scripts.Data;
using _Scripts.Data.Tool;
using _Scripts.SaveSystem;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using Grid_Map;
using UnityEngine;

namespace _Scripts.Managers
{
    public class ToolManager : Singleton<ToolManager>
    {

        private GridSpawner _gridSpawner;
        private ShellManager _shellManager;
        private readonly Dictionary<ToolType, IToolCommand> _commands = new Dictionary<ToolType, IToolCommand>();

        private void OnEnable()
        {
            EventBus.Subscribe<PurchaseEvent>(AddUse);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<PurchaseEvent>(AddUse);
        }

        public void Initialize(GridSpawner gridSpawner, ShellManager shellManager)
        {
            _gridSpawner = gridSpawner;
            _shellManager = shellManager;
            InitCommands();
        }


        private void InitCommands()
        {
            _commands.Clear();
            
            _commands[ToolType.Shuffle] = new ShuffleTool(_gridSpawner, DataSystem.LoadToolUse(ToolType.Shuffle),3);
            _commands[ToolType.AddSlot] = new AddSlotTool(_shellManager, DataSystem.LoadToolUse(ToolType.AddSlot),1);
            _commands[ToolType.Return]  = new ReturnTool(_shellManager,  DataSystem.LoadToolUse(ToolType.Return),3);
        }

        public void UseShuffle() => Execute(ToolType.Shuffle);
        public void UseAddSlot() => Execute(ToolType.AddSlot);
        public void UseReturn() => Execute(ToolType.Return);

        private void Execute(ToolType toolType)
        {
            if (!_commands.TryGetValue(toolType, out var command)) return;

            if (GetUseLeft(toolType) == 0)
            {
                EventBus.Publish(new OutOfToolUseEvent(toolType));
                return;
            }

            command.Execute();
            DataSystem.SaveToolUse(toolType, command.UseLeft);
        }

        public void ResetForNewLevel() => InitCommands();

        private int GetUseLeft(ToolType toolType) =>
            _commands.TryGetValue(toolType, out var command) ? command.UseLeft : 0;

        private void AddUse(PurchaseEvent evt)
        {
            if (!_commands.TryGetValue(evt.ToolType, out var command)) return;
            command.AddingUses(evt.Amount);
            DataSystem.SaveToolUse(evt.ToolType, command.UseLeft);
        }
    }
}