using _Scripts.Core.Tile;
using _Scripts.Data.Sounds;
using _Scripts.Systems;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Managers
{
    public class GameInputManager : PersistentSingleton<GameInputManager>
    {
        private static float _lastClickTime = 0f;
        private readonly float clickCooldown = 0.1f;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time - _lastClickTime >= clickCooldown)
                {
                    HandlePlayerClick();
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void HandlePlayerClick()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider == null) return;

                TileCell clickedTile = hit.collider.GetComponent<TileCell>();
                if (clickedTile == null || clickedTile.IsClicked) return;
                _lastClickTime = Time.time;
                clickedTile.IsClicked = true;
                hit.collider.enabled = false;
                if (clickedTile.isHintTileCell) EventBus.Publish(new ClickedHintedTileCellEvent(clickedTile));
                EventBus.Publish(new PlaySoundEvent(SoundType.TileSound));
                EventBus.Publish(new TileClickEvent(clickedTile));
            }
        }
    }
}