using _Scripts.Data;
using _Scripts.Managers;
using Grid_Map;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Core.Tools
{
    public interface IToolCommand
    {
        public int UseLeft { get; }
        public void Execute();
        public bool CanExecute();
        public void Undo(){}
        public void AddingUses(int amount);
    }
}