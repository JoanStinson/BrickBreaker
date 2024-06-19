using System;
using UnityEngine;

namespace JGM.Game
{
    public class ExtraBallView : MonoBehaviour
    {
        public Action OnPickup { get; set; }

        [SerializeField]
        private ParticleSystem m_particleSystem;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnPickup?.Invoke();
            DestroyBall();
        }

        public void DestroyBall()
        {
            gameObject.SetActive(false);
            m_particleSystem.Play();
        }
    }
}