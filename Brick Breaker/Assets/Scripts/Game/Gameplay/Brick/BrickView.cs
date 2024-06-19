using TMPro;
using UnityEngine;

namespace JGM.Game
{
    public class BrickView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private ParticleSystem m_particleSystem;
        [SerializeField] private TextMeshProUGUI m_healthText;
        [SerializeField] private float m_healthAmount;
        [SerializeField] private Color m_baseColor;

        private GameModel m_gameModel;

        public void Initialize(GameModel gameModel)
        {
            m_gameModel = gameModel;
            OnEnable();
        }

        private void OnEnable()
        {
            m_healthAmount = m_gameModel?.LevelOfFinalBrick ?? 1;
            m_healthText.text = m_healthAmount.ToString();
            ChangeColor();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            m_healthAmount--;
            m_healthText.text = m_healthAmount.ToString();
            ChangeColor();

            if (m_healthAmount <= 0)
            {
                Color color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, 0.5f);
                var mainModule = m_particleSystem.main;
                mainModule.startColor = color;
                m_particleSystem.Play();
                gameObject.SetActive(false);
            }
        }

        public void ChangeColor()
        {
            float t = m_healthAmount / m_gameModel?.LevelOfFinalBrick ?? 1;
            m_spriteRenderer.color = Color.LerpUnclamped(m_baseColor, Color.red, t);
        }
    }
}