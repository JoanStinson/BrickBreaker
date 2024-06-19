using System;
using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class BallView : MonoBehaviour
    {
        public Action<BallView> OnBallReturned { get; set; }
        public class Factory : PlaceholderFactory<BallView> { }

        [SerializeField] private Rigidbody2D m_rigidbody2D;
        [SerializeField] private CircleCollider2D m_circleCollider2D;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private float m_moveSpeed = 7f;
        [SerializeField] private float m_minimumYPosition = -4.09f;

        private BallLauncherView m_ballLauncherView;

        public void Initialize(BallLauncherView ballLauncherView)
        {
            m_ballLauncherView = ballLauncherView;
            m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        private void Update()
        {
            if (m_rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
            {
                MoveBall();
            }
        }

        private void MoveBall()
        {
            m_rigidbody2D.velocity = m_rigidbody2D.velocity.normalized * m_moveSpeed;

            if (transform.localPosition.y < m_minimumYPosition)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, m_minimumYPosition, 0);
                DisableBallPhysics();

                Vector3 firstCollisionPoint = m_ballLauncherView.UpdateFirstCollisionPoint();
                float time = Vector2.Distance(transform.position, firstCollisionPoint) / 5.0f;
                string onCompleteMethod = nameof(OnReturnedToStartPosition);
                MoveBallTo(firstCollisionPoint, iTween.EaseType.linear, time, onCompleteMethod);
            }
        }

        public void DisableBallPhysics()
        {
            m_circleCollider2D.enabled = false;
            m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        public void MoveBallTo(Vector3 position, iTween.EaseType easeType, float time, string onCompleteMethod = nameof(HideBall))
        {
            iTween.Stop(gameObject);

            if (m_spriteRenderer.enabled)
            {
                var args = iTween.Hash("position", position, "easetype", easeType, "time", time, "oncomplete", onCompleteMethod);
                iTween.MoveTo(gameObject, args);
            }
        }

        private void OnReturnedToStartPosition()
        {
            OnBallReturned?.Invoke(this);
        }

        public void ShootBall(Vector2 direction)
        {
            m_spriteRenderer.enabled = true;
            m_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            m_circleCollider2D.enabled = true;
            m_rigidbody2D.AddForce(direction);
        }

        public void HideBall()
        {
            m_spriteRenderer.enabled = false;
        }

        public void DisableBall()
        {
            m_spriteRenderer.enabled = false;
            m_circleCollider2D.enabled = false;
            m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
    }
}