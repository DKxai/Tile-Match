using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class GameInputManager : MonoBehaviour
{
    private static float lastClickTime = 0f;
    private float clickCooldown = 0.1f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime >= clickCooldown)
            {
                HandlePlayerClick();
            }
        }
    }

    private void HandlePlayerClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider == null) return;

        TileCell clickedTile = hit.collider.GetComponent<TileCell>();
        if (clickedTile == null || clickedTile.IsClicked) return;
        lastClickTime = Time.time;
        clickedTile.IsClicked = true;
        hit.collider.enabled = false;
        TileEventBus.TileClicked(clickedTile);
    }
}