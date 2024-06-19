using UnityEngine;

namespace JGM.Game
{
    public class BallView : MonoBehaviour
    {
        public static Vector3 FirstCollisionPoint { private set; get; }

        [SerializeField] private Rigidbody2D m_rigidbody2D;
        [SerializeField] private CircleCollider2D m_circleCollider2D;
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField] private float m_moveSpeed = 7f;
        [SerializeField] private float m_MinimumYPosition = -4.09f;

        private static int m_returnedBallsAmount = 0;

        private void Awake()
        {
            m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        private void Update()
        {
            if (m_rigidbody2D.bodyType != RigidbodyType2D.Dynamic)
            {
                return;
            }

            m_rigidbody2D.velocity = m_rigidbody2D.velocity.normalized * m_moveSpeed;

            if (transform.localPosition.y < m_MinimumYPosition)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, m_MinimumYPosition, 0);

                if (FirstCollisionPoint == Vector3.zero)
                {
                    FirstCollisionPoint = transform.position;
                    BallLauncherView.Instance.m_BallSprite.transform.position = FirstCollisionPoint;
                    BallLauncherView.Instance.m_BallSprite.enabled = true;
                }

                DisablePhysics();
                MoveTo(FirstCollisionPoint, iTween.EaseType.linear, Vector2.Distance(transform.position, FirstCollisionPoint) / 5.0f, "Deactive");
            }
        }

        public void DisablePhysics()
        {
            m_circleCollider2D.enabled = false;
            m_rigidbody2D.bodyType = RigidbodyType2D.Static;
        }

        public void MoveTo(Vector3 position, iTween.EaseType easeType, float time, string onCompleteMethod)
        {
            iTween.Stop(gameObject);

            if (m_spriteRenderer.enabled)
            {
                var args = iTween.Hash("position", position, "easetype", easeType, "time", time, "oncomplete", onCompleteMethod);
                iTween.MoveTo(gameObject, args);
            }
        }

        private static void ContinuePlaying()
        {
            if (FirstCollisionPoint != Vector3.zero)
            {
                BallLauncherView.Instance.transform.position = FirstCollisionPoint;
            }

            BallLauncherView.Instance.m_BallSprite.enabled = true;
            BallLauncherView.Instance.ActivateHUD();
            BallLauncherView.Instance.OnReturnBallsToNewStartPosition?.Invoke();
            BrickRowSpawnerView.Instance.MoveDownRows();
            BrickRowSpawnerView.Instance.SpawnBricks();
            FirstCollisionPoint = Vector3.zero;
            m_returnedBallsAmount = 0;
            BallLauncherView.Instance.m_CanPlay = true;
        }

        public static void ResetFirstCollisionPoint()
        {
            FirstCollisionPoint = Vector3.zero;
        }

        public static void ResetReturningBallsAmount()
        {
            m_returnedBallsAmount = 0;
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

        private void Deactive()
        {
            m_returnedBallsAmount++;

            if (m_returnedBallsAmount == BallLauncherView.Instance.m_BallsAmount)
            {
                ContinuePlaying();
            }

            m_spriteRenderer.enabled = false;
        }

        private void DeactiveSprite()
        {
            m_spriteRenderer.enabled = false;
        }
    }
}