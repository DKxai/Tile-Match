using System.Collections.Generic;
using System.Linq;
using _Scripts.Core.Tile;
using _Scripts.Data;
using _Scripts.Data.Tool;
using _Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Core.Tools
{
    public class ReturnTool : BaseToolCommand
    {
        private int _tilesInReturnShell = 0;
        private readonly ShellManager _shellManager;
        private List<TileCell> _returnTiles = new List<TileCell>(3);
        
        protected override ToolType ToolType => ToolType.Return;
        public ReturnTool(ShellManager shellManager, int useLeft, int useLeftInALevel) : base(useLeft, useLeftInALevel)
        {
            _shellManager = shellManager;
        }


        /// <summary>
        /// Move To Return Shell
        /// Currently, get stuck: Move to the return shell:
        /// -> Remove InShell
        /// -> filled Shell
        /// -> Dictionary
        /// -> SlotsFilled
        /// -> Collider Clicked
        /// </summary>
        public override void Execute()
        {
            if (!CanExecute())
                return;

            int count = Mathf.Min(3, _shellManager.tilesInShell.Count);

            _returnTiles = _shellManager.tilesInShell
                .TakeLast(count)
                .ToList();
            _shellManager.MoveToReturnShell(_returnTiles,_tilesInReturnShell);
            _tilesInReturnShell += _returnTiles.Count;

          ComsumeUse();
        }

        public override bool CanExecute() => _shellManager != null && _useLeft > 0 && _useLeftInALevel > 0;
    }
}