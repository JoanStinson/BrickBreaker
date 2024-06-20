using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class MultiplierView : ScreenView
    {
        [SerializeField] private Button m_x1Button;
        [SerializeField] private Button m_x3Button;
        [SerializeField] private Button m_x5Button;
        
        private GameView m_gameView;

        public override void Initialize(GameView gameView)
        {
            m_gameView = gameView;
            m_x1Button.onClick.AddListener(OnClick1XButton);
            m_x3Button.onClick.AddListener(OnClick3XButton);
            m_x5Button.onClick.AddListener(OnClick5XButton);
        }

        private void OnClick1XButton()
        {
            m_gameView.OnClick1XButton();
        }

        private void OnClick3XButton()
        {
            m_gameView.OnClick3XButton();
        }

        private void OnClick5XButton()
        {
            m_gameView.OnClick5XButton();
        }
    }
}
