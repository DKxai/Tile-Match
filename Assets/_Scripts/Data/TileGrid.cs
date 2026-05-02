using System;

namespace Grid_Map
{
    [Serializable]
    public class TileGrid
    {
        public int Level  { get; private set; }
        public int Width  { get; private set; }
        public int Height { get; private set; }
        public int Layers { get; private set; }
        public int[] Grid { get; private set; }

        public int GetWidth()  => Width;
        public int GetHeight() => Height;
        public int GetLayers() => Layers;

        public TileGrid(int level, int width, int height, int layers)
        {
            Level  = level;
            Width  = width;
            Height = height;
            Layers = layers;
            Grid   = new int[width * height * layers];
        }

        public int GetValue(int x, int y, int z) => Grid[Index(x, y, z)];

        public void SetValue(int x, int y, int z, int value) => Grid[Index(x, y, z)] = value;

        public GridData Save() => new GridData
        {
            level  = Level,
            width  = Width,
            height = Height,
            layers = Layers,
            data   = Grid
        };

        public static TileGrid Load(GridData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var grid = new TileGrid(data.level, data.width, data.height, data.layers);
            Array.Copy(data.data, grid.Grid, data.width * data.height * data.layers);
            return grid;
        }

        private int Index(int x, int y, int z) => x + y * Width + z * Width * Height;
    }
}