using UnityEngine;
using UnityEngine.EventSystems;

public class GameInputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandlePlayerClick();
        }
    }

    private void HandlePlayerClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            TileCell clickedTile = hit.collider.GetComponent<TileCell>();

            if (clickedTile != null && !clickedTile.IsClicked)
            {
                clickedTile.IsClicked = true;
                ShellManager.Instance.AddTile(clickedTile);
                hit.collider.enabled = false;
            }
        }
    }
}