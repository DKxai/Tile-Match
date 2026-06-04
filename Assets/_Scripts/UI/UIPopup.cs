using UnityEngine;

namespace _Scripts.UI
{
    public class UIPopup:MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
            
            PlayShowAnimation();
        }

        protected virtual void Hide()
        {
            PlayHideAnimation();
            
        }

        protected virtual void PlayShowAnimation()
        {
            
        }

        protected virtual void PlayHideAnimation()
        {
            gameObject.SetActive(false);
        }
    }
}