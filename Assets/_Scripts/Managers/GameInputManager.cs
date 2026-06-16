using _Scripts.Core.Tile;
using _Scripts.Data.Sounds;
using _Scripts.Systems;
using _Scripts.Utils;
using _Scripts.Utils.Event_Bus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Scripts.Managers
{
    public class GameInputManager : PersistentSingleton<GameInputManager>
    {
        private static float _lastClickTime;
        private readonly float clickCooldown = 0.1f;

        private void Update()
        {
            if (!IsPointerPressed())
                return;

            if (Time.time - _lastClickTime < clickCooldown)
                return;

            HandlePlayerClick();
        }

        private bool IsPointerPressed()
        {
            // PC
            if (Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
                return true;

            // Android / iOS
            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
                return true;

            return false;
        }

        private Vector2 GetPointerPosition()
        {
            if (Touchscreen.current != null)
                return Touchscreen.current.primaryTouch.position.ReadValue();

            if (Mouse.current != null)
                return Mouse.current.position.ReadValue();

            return Vector2.zero;
        }

        private void HandlePlayerClick()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Camera.main == null)
                return;

            Vector2 screenPos = GetPointerPosition();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            Collider2D hit = Physics2D.OverlapPoint(worldPos);

            if (hit == null)
                return;

            TileCell clickedTile = hit.GetComponent<TileCell>();

            if (clickedTile == null || clickedTile.IsClicked)
                return;

            _lastClickTime = Time.time;

            clickedTile.IsClicked = true;
            hit.enabled = false;

            if (clickedTile.isHintTileCell)
                EventBus.Publish(new ClickedHintedTileCellEvent(clickedTile));

            EventBus.Publish(new PlaySoundEvent(SoundType.TileSound));
            EventBus.Publish(new TileClickEvent(clickedTile));
        }
    }
}