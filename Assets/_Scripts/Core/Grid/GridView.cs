using Grid_Map;
using UnityEngine;

namespace _Scripts.Core.Grid
{
    public class GridView : MonoBehaviour
    {
        [Header("Grid Size")]
        [SerializeField] int width = 8;
        [SerializeField] int height = 8;
        [SerializeField] int layer = 5;

        [Header("Cell")]
        [SerializeField] float cellSize = 0.6f;

        [Header("Slider")]
        [SerializeField] UnityEngine.UI.Slider slider;

        private TileGrid _tileGrid;
        private bool[] _layerVisible;
        public int CurrentLayer { get; private set; }
        public float CellSize => cellSize;
        public int GetLayerWidth(int z)  => z % 2 == 0 ? width + 1 : width;
        public int GetLayerHeight(int z) => z % 2 == 0 ? height + 1 : height;

        void Start()
        {
            if (slider == null) return;
            slider.minValue = 0;
            slider.maxValue = layer - 1;
            slider.wholeNumbers = true;
            slider.value = layer - 1;
            slider.onValueChanged.AddListener(OnLayerChanged);
            OnLayerChanged(slider.value);
        }

     


        public void LoadGrid(TileGrid grid)
        {
            _tileGrid = grid;
            
            width  = grid.GetWidth()  - 1;
            height = grid.GetHeight() - 1;
            layer  = grid.GetLayers();

            _layerVisible = new bool[layer];
            for (int i = 0; i < layer; i++)
                _layerVisible[i] = true;

            if (slider != null)
            {
                slider.minValue = 0;
                slider.maxValue = layer - 1;
                slider.wholeNumbers = true;
                slider.value = layer - 1;
            }

            CurrentLayer = layer - 1;
            UpdateLayerVisibility();
        }

        private void OnLayerChanged(float value)
        {
            CurrentLayer = (int)value;
            UpdateLayerVisibility();
        }

        void UpdateLayerVisibility()
        {
            if (_layerVisible == null) return;
            for (int i = 0; i < _layerVisible.Length; i++)
                _layerVisible[i] = false;

            if (CurrentLayer < _layerVisible.Length)
                _layerVisible[CurrentLayer] = true;
            if (CurrentLayer - 1 >= 0 && CurrentLayer - 1 < _layerVisible.Length)
                _layerVisible[CurrentLayer - 1] = true;
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (_tileGrid == null || Camera.main == null) return;

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 20;

            Vector3 origin = transform.position;

            int totalLayers = _tileGrid.Layers;
            for (int z = 0; z < totalLayers; z++)
            {
                if (_layerVisible == null || z >= _layerVisible.Length || !_layerVisible[z]) continue;

                Color c = GetLayerColor(z);
                c.a = (z == CurrentLayer) ? 1f : 0.6f;
                style.normal.textColor = c;

                int lw = GetLayerWidth(z);
                int lh = GetLayerHeight(z);
                Vector3 offset = z % 2 == 1
                    ? new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0)
                    : Vector3.zero;

                for (int x = 0; x < lw; x++)
                {
                    for (int y = 0; y < lh; y++)
                    {
                        int val = _tileGrid.GetValue(x, y, z);
                        if (val == 0) continue;

                        Vector3 wp = GetCellCenter(origin, x, y) + offset;
                        Vector3 sp = Camera.main.WorldToScreenPoint(wp);
                        if (sp.z < 0) continue;
                        sp.y = Screen.height - sp.y;
                        GUI.Label(new Rect(sp.x - 25, sp.y - 25, 50, 50), val.ToString(), style);
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            if (_tileGrid == null) return;

            int totalLayers = _tileGrid.Layers;
            Vector3 origin = transform.position;

            for (int z = 0; z < totalLayers; z++)
            {
                if (_layerVisible == null || z >= _layerVisible.Length || !_layerVisible[z]) continue;

                Color c = GetLayerColor(z);
                c.a = (z == CurrentLayer) ? 1f : 0.6f;
                Gizmos.color = c;

                int lw = GetLayerWidth(z);
                int lh = GetLayerHeight(z);
                Vector3 offset = z % 2 == 1
                    ? new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0)
                    : Vector3.zero;

                for (int x = 0; x < lw; x++)
                {
                    for (int y = 0; y < lh; y++)
                    {
                        Vector3 pos = GetWorldPosition(origin, x, y) + offset;
                        Gizmos.DrawLine(pos, GetWorldPosition(origin, x + 1, y) + offset);
                        Gizmos.DrawLine(pos, GetWorldPosition(origin, x, y + 1) + offset);
                    }
                }

                Gizmos.DrawLine(
                    GetWorldPosition(origin, lw, 0) + offset,
                    GetWorldPosition(origin, lw, lh) + offset);
                Gizmos.DrawLine(
                    GetWorldPosition(origin, 0, lh) + offset,
                    GetWorldPosition(origin, lw, lh) + offset);
            }
        }
#endif

        public void ToggleLayer(int layerIndex)
        {
            if (_layerVisible == null || layerIndex < 0 || layerIndex >= layer) return;
            _layerVisible[layerIndex] = !_layerVisible[layerIndex];
        }

        Vector3 GetWorldPosition(Vector3 origin, int x, int y)
            => origin + new Vector3(x, y, 0) * cellSize;

        Vector3 GetCellCenter(Vector3 origin, int x, int y)
            => origin + new Vector3(x + 0.5f, y + 0.5f, 0) * cellSize;

        Color GetLayerColor(int z)
        {
            switch (z)
            {
                case 0: return new Color(0.9f, 0.3f, 0.3f);
                case 1: return new Color(1.0f, 0.6f, 0.2f);
                case 2: return new Color(0.3f, 0.8f, 0.4f);
                case 3: return new Color(0.3f, 0.6f, 1.0f);
                case 4: return new Color(0.7f, 0.4f, 1.0f);
                default: return Color.white;
            }
        }
    }
}