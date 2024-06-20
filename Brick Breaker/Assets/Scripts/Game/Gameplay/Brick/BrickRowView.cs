using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace JGM.Game
{
    public class BrickRowView : MonoBehaviour
    {
        public Action OnBrickGotHit { get; set; }
        public Action OnPickupBallFromRow { get; set; }
        public Action OnBrickRowTouchedFloor { get; set; }
        public class Factory : PlaceholderFactory<BrickRowView> { }

        [SerializeField] private float m_floorPosition = -3.8f;
        [SerializeField] private BrickView[] m_bricks;
        [SerializeField] private ExtraBallView[] m_extraBalls;

        private float m_spawningRowDistance;
        private float m_spawningTopPosition;

        public void Initialize(float spawningRowDistance, float spawningTopPosition, GameModel gameModel)
        {
            m_spawningRowDistance = spawningRowDistance;
            m_spawningTopPosition = spawningTopPosition;

            foreach (var brick in m_bricks)
            {
                brick.Initialize(gameModel);
                brick.OnBrickHit += OnBrickHit;
            }

            foreach (var extraBall in m_extraBalls)
            {
                extraBall.OnPickup += OnPickup;
            }

            OnEnable();
        }

        private void OnBrickHit()
        {
            OnBrickGotHit?.Invoke();
        }

        private void OnPickup()
        {
            OnPickupBallFromRow?.Invoke();
        }

        private void OnEnable()
        {
            if (m_spawningRowDistance == 0)
            {
                return;
            }

            GoToTop();
            MoveDown(m_spawningRowDistance);
            m_extraBalls[Random.Range(0, m_extraBalls.Length)].gameObject.SetActive(true);
            RandomizeBricks();
            MakeAtLeastOneBrickAvailable();
        }

        private void GoToTop()
        {
            HideAllRows();
            transform.localPosition = new Vector3(0, m_spawningTopPosition, 0);
        }

        public void MoveDown(float howMuch)
        {
            for (int i = 0; i < m_bricks.Length; i++)
            {
                if (m_bricks[i].gameObject.activeInHierarchy)
                {
                    m_bricks[i].ChangeColor();
                }
            }

            var position = new Vector3(transform.position.x, transform.position.y - howMuch, transform.position.z);
            iTween.MoveTo(gameObject, position, 0.25f);
        }

        private void RandomizeBricks()
        {
            for (int i = 0; i < m_bricks.Length; i++)
            {
                if (m_extraBalls[i].gameObject.activeInHierarchy)
                {
                    m_bricks[i].gameObject.SetActive(false);
                }
                else
                {
                    m_bricks[i].gameObject.SetActive(Random.Range(0, 2) == 1);
                }
            }
        }

        private void MakeAtLeastOneBrickAvailable()
        {
            bool hasNoBrick = true;

            for (int i = 0; i < m_bricks.Length; i++)
            {
                if (m_bricks[i].gameObject.activeInHierarchy)
                {
                    hasNoBrick = false;
                    break;
                }
            }

            if (hasNoBrick)
            {
                for (int i = 0; i < m_bricks.Length; i++)
                {
                    if (!m_extraBalls[i].gameObject.activeInHierarchy)
                    {
                        m_bricks[i].gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

        private void HideAllRows()
        {
            for (int i = 0; i < m_bricks.Length; i++)
            {
                m_bricks[i].gameObject.SetActive(false);
                m_extraBalls[i].gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (transform.localPosition.y <= m_floorPosition)
            {
                if (HasActiveBricks())
                {
                    OnBrickRowTouchedFloor?.Invoke();
                }
                else
                {
                    GoToTop();
                    gameObject.SetActive(false);
                }
            }
        }

        public bool HasActiveBricks()
        {
            bool hasActiveBrick = false;

            for (int i = 0; i < m_bricks.Length; i++)
            {
                if (m_bricks[i].gameObject.activeInHierarchy)
                {
                    hasActiveBrick = true;
                    break;
                }
            }

            return hasActiveBrick;
        }

        public void CheckActiveScoreBall()
        {
            for (int i = 0; i < m_extraBalls.Length; i++)
            {
                if (m_extraBalls[i].gameObject.activeInHierarchy)
                {
                    m_extraBalls[i].DestroyBall();
                    OnPickupBallFromRow?.Invoke();
                    break;
                }
            }
        }
    }
}