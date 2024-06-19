using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class BrickView : MonoBehaviour
    {
        [SerializeField] private Text m_healthText;
        [SerializeField] private int m_healthAmount;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private ParticleSystem m_particleSystem;

        private void OnEnable()
        {
            m_healthAmount = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick;
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
                // 1 - play a particle
                Color color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, 0.5f);
                var mainModule = m_particleSystem.main;
                mainModule.startColor = color;
                m_particleSystem.Play();

                // 2 - hide this Brick or this row
                gameObject.SetActive(false);
            }
        }

        public void ChangeColor()
        {
            m_spriteRenderer.color = Color.LerpUnclamped(new Color(1, 0.75f, 0, 1), Color.red, m_healthAmount / (float)BrickRowSpawnerView.Instance.m_LevelOfFinalBrick);
        }
    }
}