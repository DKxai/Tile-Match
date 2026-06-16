using _Scripts.Managers;
using DG.Tweening;
using Grid_Map;
using UnityEngine;

namespace _Scripts.Core.Tile
{
    public class TileCell : MonoBehaviour
    {
        [Header("Tile info")] public Transform iconSprite;
        public int ID;

        public bool IsClicked = false;

        [Header("Grid Coordinates")] public int gridX;
        public int gridY;
        public int gridZ;
        public Vector3 worldPos;
        private Collider2D col;
        private SpriteRenderer[] sr;
        public bool isHintTileCell = false;
        public Vector3Int GridPos() => new Vector3Int(gridX, gridY, gridZ);
        public bool IsBlocked { get; private set; }

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            sr = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 10, 0.8f);
        }

        public void SetupCell(int x, int y, int layer, Vector3 worldPosition)
        {
            gridX = x;
            gridY = y;
            gridZ = layer;
            worldPos = worldPosition;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -layer;
        }

        public void RefreshState(TileGrid grid)
        {
            if (IsClicked) return;

            IsBlocked = LevelManager.Instance.RuleValidator.isTileBlocked(grid, gridX, gridY, gridZ);

            if (col != null) col.enabled = !IsBlocked;

            float brightness = IsBlocked ? 0.5f : 1f;

            foreach (var s in sr)
            {
                if (s != null)
                {
                    Color c = s.color;

                    // s.color = new Color(brightness, brightness, brightness, c.a);
                    s.DOKill();
                    s.DOColor(new Color(brightness, brightness, brightness, c.a), 0.35f).SetDelay(0.15f);

                }
            }
        }

        public void MoveToWorldPosition(Vector3 tileWorldPos)
        {
            gameObject.transform.position = tileWorldPos;
        }
    }
}