using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace JGM.Game
{
    public abstract class ScreenView : MonoBehaviour
    {
        [SerializeField]
        protected CanvasGroup m_canvasGroup;

        private const int m_showPositionInX = -2000;
        private const int m_hidePositionInX = 2000;
        private const float m_animationDuration = 1f;

        public abstract void Initialize(GameView gameView);

        public virtual async void Show()
        {
            m_canvasGroup.blocksRaycasts = false;
            var rectTransform = (RectTransform)gameObject.transform;
            rectTransform.DOAnchorPos(new Vector2(m_showPositionInX, 0), 0);
            rectTransform.DOAnchorPos(Vector2.zero, m_animationDuration);
            gameObject.SetActive(true);

            await Task.Delay(TimeSpan.FromSeconds(m_animationDuration));
            m_canvasGroup.blocksRaycasts = true;
        }

        public virtual async void Hide()
        {
            m_canvasGroup.blocksRaycasts = false;
            var rectTransform = (RectTransform)gameObject.transform;
            rectTransform.DOAnchorPos(new Vector2(m_hidePositionInX, 0), m_animationDuration);

            await Task.Delay(TimeSpan.FromSeconds(m_animationDuration));
            gameObject.SetActive(false);
        }
    }
}
