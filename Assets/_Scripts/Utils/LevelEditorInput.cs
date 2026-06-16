#if UNITY_EDITOR
using _Scripts.Core.Grid;
using _Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Grid_Map;

public class LevelEditorInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GridView gridView;

    private void Update()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (levelManager == null || levelManager.CurrentGrid == null) return;
        if (gridView == null) return;

        if (Mouse.current == null ||
            !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (Camera.main == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Plane plane = new Plane(Vector3.forward, gridView.transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPos = ray.GetPoint(distance);
            Vector3 localPos = worldPos - gridView.transform.position;

            int currentLayer = gridView.CurrentLayer;
            float cellSize = gridView.CellSize;

            GetXY(localPos, currentLayer, cellSize, out int x, out int y);

            int lw = gridView.GetLayerWidth(currentLayer);
            int lh = gridView.GetLayerHeight(currentLayer);

            if (x >= 0 && x < lw && y >= 0 && y < lh)
            {
                TileGrid currentGrid = levelManager.CurrentGrid;

                int currentVal = currentGrid.GetValue(x, y, currentLayer);
                int newVal = currentVal == 0 ? 1 : 0;

                currentGrid.SetValue(x, y, currentLayer, newVal);

                Debug.Log(
                    $"[LevelEditor] SetValue ({x}, {y}, Layer: {currentLayer}) = {newVal}");
            }
        }
    }

    private void GetXY(Vector3 localPos, int z, float cellSize, out int x, out int y)
    {
        Vector3 offset = (z % 2 == 1)
            ? new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0)
            : Vector3.zero;

        localPos -= offset;

        x = Mathf.FloorToInt(localPos.x / cellSize);
        y = Mathf.FloorToInt(localPos.y / cellSize);
    }
}
#endif