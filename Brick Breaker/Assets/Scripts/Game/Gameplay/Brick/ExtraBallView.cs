using UnityEngine;

namespace JGM.Game
{
    public class ExtraBallView : MonoBehaviour
    {
        [SerializeField] private BrickRowView m_brickRow;
        [SerializeField] private ParticleSystem m_particleSystem;
        [SerializeField] private Color m_particleColor;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            BallLauncherView.Instance.m_TempAmount++;
            PlayParticle();
        }

        public void PlayParticle()
        {
            gameObject.SetActive(false);
            m_particleSystem.startColor = m_particleColor;
            m_particleSystem.Play();
        }
    }
}