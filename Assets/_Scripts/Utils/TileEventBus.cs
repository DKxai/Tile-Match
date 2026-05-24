using System;
using _Scripts.Data;

namespace Utils
{
    public static class TileEventBus
    {
        public static event Action<TileCell> OnTileClicked;

        public static event Action<int> OnTileMatched;

        public static event Action OnBoardCleared;
        public static Action<ToolType, int> OnToolUsed;
        public static Action<string> OnNodeClicked;
        public static Action<ToolType> OnToolOutOfUse;
        // Settings 
        public static Action OnSettingClick;
        
        public static void TileClicked(TileCell cell) => OnTileClicked?.Invoke(cell);
        public static void TileMatched(int tileID) => OnTileMatched?.Invoke(tileID);
        public static void BoardCleared() => OnBoardCleared?.Invoke();
        
    }
}