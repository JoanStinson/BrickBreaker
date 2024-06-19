using System;
using UnityEngine;

namespace JGM.Game
{
    public class BallView : MonoBehaviour
    {
        public Action<BallView> OnBallReturned { get; set; }

        [SerializeField] private Rigidbody2D m_rigidbody2D;
        [SerializeField] private CircleCollider2D m_circleCollider2D;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private float m_moveSpeed = 7f;
        [SerializeField] private float m_minimumYPosition = -4.09f;

        private void Awake()
        {
            m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        private void Update()
        {
            if (m_rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
            {
                Move();
            }
        }

        private void Move()
        {
            m_rigidbody2D.velocity = m_rigidbody2D.velocity.normalized * m_moveSpeed;

            if (transform.localPosition.y < m_minimumYPosition)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, m_minimumYPosition, 0);

                if (BallLauncherView.Instance.FirstCollisionPoint == Vector3.zero)
                {
                    BallLauncherView.Instance.FirstCollisionPoint = transform.position;
                    BallLauncherView.Instance.m_BallSprite.transform.position = BallLauncherView.Instance.FirstCollisionPoint;
                    BallLauncherView.Instance.m_BallSprite.enabled = true;
                }

                DisablePhysics();
                float time = Vector2.Distance(transform.position, BallLauncherView.Instance.FirstCollisionPoint) / 5.0f;
                string onCompleteMethod = nameof(OnReturnedToStartPosition);
                MoveTo(BallLauncherView.Instance.FirstCollisionPoint, iTween.EaseType.linear, time, onCompleteMethod);
            }
        }

        public void DisablePhysics()
        {
            m_circleCollider2D.enabled = false;
            m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        public void MoveTo(Vector3 position, iTween.EaseType easeType, float time, string onCompleteMethod = nameof(HideBall))
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

        public void HideBall()
        {
            m_spriteRenderer.enabled = false;
        }

        public void GetReadyAndAddForce(Vector2 direction)
        {
            m_spriteRenderer.enabled = true;
            m_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            m_circleCollider2D.enabled = true;
            m_rigidbody2D.AddForce(direction);
        }

        public void Disable()
        {
            m_spriteRenderer.enabled = false;
            m_circleCollider2D.enabled = false;
            m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
    }
}