using System;
using System.Collections.Generic;
using _Scripts.Core.Tools;
using Grid_Map;
using UnityEngine;

namespace _Scripts.Managers
{
    public class ToolManager: Singleton<ToolManager>
    {
        [Header("Invoker & Receiver")]
        [SerializeField] private GridSpawner gridSpawner;
        [SerializeField] private ShellManager shellManager;
        [SerializeField] private ToolData toolData;

        private readonly Dictionary<string, IToolCommand> _commands = new Dictionary<string, IToolCommand>();

        private void Start() => Init();

        private void Init()
        {
            _commands.Clear();
          _commands["Shuffle"] = new ShuffleTool(gridSpawner,toolData.shuffleUseInALevel);
            _commands["AddSlot"] = new AddSlotTool(shellManager,toolData.addSlotUseInALevel);
            _commands["Hint"] = new HintTool();
          _commands["Return"] = new ReturnTool(shellManager,3);
        }

        public void UseShuffle() => Execute("Shuffle");
        public void UseAddSlot()=> Execute("AddSlot");
        public void UseHint()=> Execute("Hint");
        public void UseReturn() => Execute("Return");

        private void Execute(string commandStr)
        {
            if (!_commands.TryGetValue(commandStr,out var command)) return;

            if (!command.CanExecute())
            {
                 Debug.LogWarning("[ToolManager] Command not Execute: " + commandStr);
                 return;
            }
            command.Execute();
            
        }

        public void ResetForNewLevel() => Init();

        public int GetUseLeft(string commandStr) =>
            _commands.TryGetValue(commandStr, out var command) ? command.UseLeft : 0;
    }
}