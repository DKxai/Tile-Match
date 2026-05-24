using System.Collections.Generic;
using System.Linq;
using _Scripts.Data;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace _Scripts.Core.Tools
{
    public class ReturnTool : IToolCommand
    {
        public int tilesInReturnShell = 0;
        private ShellManager _shellManager;
        private int _useLeft;
        private List<TileCell> returnTiles = new List<TileCell>(3);
        
        public ReturnTool(ShellManager shellManager, int useLeft)
        {
            _shellManager = shellManager;
            _useLeft = useLeft;
        }

        public int UseLeft => _useLeft;
        
        /// <summary>
        /// Move To Return Shell
        /// Currently, get stuck: Move to the return shell:
        /// -> Remove InShell
        /// -> filled Shell
        /// -> Dictionary
        /// -> SlotsFilled
        /// -> Collider Clicked
        /// </summary>
        public void Execute()
        {
            if (!CanExecute())
                return;

            int count = Mathf.Min(3, _shellManager.tilesInShell.Count);

            returnTiles = _shellManager.tilesInShell
                .TakeLast(count)
                .ToList();
            _shellManager.MoveToReturnShell(returnTiles,tilesInReturnShell);
            tilesInReturnShell += returnTiles.Count;

            _useLeft--;

            TileEventBus.OnToolUsed?.Invoke(ToolType.Return, _useLeft);
        }

        public bool CanExecute() => _shellManager != null && _useLeft >= 0;
    }
}