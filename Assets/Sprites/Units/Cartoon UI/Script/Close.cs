using UnityEngine;

namespace Sprites.Units.Cartoon_UI.Script
{
    public class Close : MonoBehaviour
    {
        public GameObject CloseObject;
        public void CloseFunc()
        {
            CloseObject.SetActive(false);
        }
    }
}
