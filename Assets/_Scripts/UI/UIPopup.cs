using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI
{
    public class UIPopup:MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] protected RectTransform panel;
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected float showDuration = 0.35f;
        [SerializeField] protected float hideDuration = 0.35f;

        private Vector2 _shownPosition;
        private float _hiddenY;
        protected bool IsInitialized;

        protected virtual void Awake()
        {
            _shownPosition = panel.anchoredPosition;
            _hiddenY = _shownPosition.y - panel.rect.height - 100f;
        }
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
            panel.DOKill(true);
            canvasGroup.DOKill(true);

            canvasGroup.alpha = 0f;
            panel.anchoredPosition = new Vector2(_shownPosition.x, _hiddenY);

            canvasGroup.DOFade(1f, showDuration).SetUpdate(true);
            panel.DOAnchorPos(_shownPosition, showDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }

        protected virtual void PlayHideAnimation()
        {
            panel.DOKill(true);
            canvasGroup.DOKill(true);

            DOTween.Sequence()
                .Join(canvasGroup.DOFade(0f, hideDuration))
                .Join(panel.DOAnchorPosY(_hiddenY, hideDuration).SetEase(Ease.InBack))
                .OnComplete(() => gameObject.SetActive(false))
                .SetUpdate(true);
        }
    }
}