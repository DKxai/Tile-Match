using UnityEngine;

namespace _Scripts.Utils
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FitBackgroundToCamera : MonoBehaviour
    {
        public Camera targetCamera;

        void Start()
        {
            Fit();
        }
    
        void Fit()
        {
            if (targetCamera == null)
                targetCamera = Camera.main;

            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            float worldScreenHeight = targetCamera.orthographicSize * 2f;
            float worldScreenWidth = worldScreenHeight * targetCamera.aspect;

            Vector2 spriteSize = sr.sprite.bounds.size;

            transform.localScale = new Vector3(
                worldScreenWidth / spriteSize.x,
                worldScreenHeight / spriteSize.y,
                1f
            );

            transform.position = new Vector3(
                targetCamera.transform.position.x,
                targetCamera.transform.position.y,
                transform.position.z
            );
        }
    }
}