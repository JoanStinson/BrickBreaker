using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class MessageView : MonoBehaviour
    {
        [SerializeField] private RectTransform m_messagePanel;
        [SerializeField] private LocalizedText m_messageText;
        [SerializeField] private float m_durationSeconds = 3f;
        [SerializeField] private int m_animationPositionInY = 2000;
        [SerializeField] private float m_animationDuration = 1f;
        [Inject] private IAudioService m_audioService;

        public async void ShowMessage(int playerId, Action onMessageHide)
        {
            m_messageText.SetIntegerValue(playerId);
            m_messagePanel.DOAnchorPos(new Vector2(0, m_animationPositionInY), 0);
            m_messagePanel.DOAnchorPos(Vector2.zero, m_animationDuration);
            gameObject.SetActive(true);

            await Task.Delay(TimeSpan.FromSeconds(m_animationDuration / 2));
            m_audioService.Play(AudioFileNames.MessageAppearSfx);

            await Task.Delay(TimeSpan.FromSeconds(m_durationSeconds - (m_animationDuration / 2)));
            Hide();
            onMessageHide?.Invoke();
        }

        private async void Hide()
        {
            m_messagePanel.DOAnchorPos(new Vector2(0, m_animationPositionInY), m_animationDuration);
            await Task.Delay(TimeSpan.FromSeconds(m_animationDuration));
            gameObject.SetActive(false);
        }

        public void HideMessage()
        {
            gameObject.SetActive(false);
        }
    }
}
