using UnityEngine;

namespace JGM.Game
{
    public class BrickRowView : MonoBehaviour
    {
        [SerializeField] private float m_floorPosition = -3.8f;
        [SerializeField] private BrickView[] m_bricks;
        [SerializeField] private ExtraBallView[] m_extraBalls;

        private void OnEnable()
        {
            if (transform.localPosition.y < m_floorPosition)
            {
                GoToTop();
            }

            HideAll();
            GoToTop();
            MoveDown(BrickRowSpawnerView.Instance.m_SpawningRowDistance);

            // Make only one score ball available for this row randomly
            m_extraBalls[Random.Range(0, m_extraBalls.Length)].gameObject.SetActive(true);

            // Try to enable bricks randomly except at the score ball's position
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

            // make at least one brick available if there was not any one before
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

        private void GoToTop()
        {
            HideAll();
            transform.localPosition = new Vector3(0, BrickRowSpawnerView.Instance.m_SpawningTopPosition, 0);
        }

        private void HideAll()
        {
            for (int i = 0; i < m_bricks.Length; i++)
            {
                m_bricks[i].gameObject.SetActive(false);
                m_extraBalls[i].gameObject.SetActive(false);
            }
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

            iTween.MoveTo(gameObject, new Vector3(transform.position.x, transform.position.y - howMuch, transform.position.z), 0.25f);
        }

        private void Update()
        {
            if (transform.localPosition.y <= m_floorPosition)
            {
                if (HasActiveBricks())
                {
                    GameplayController.Instance.m_GameState = GameplayController.GameState.GameOver;
                }
                else if (HasActiveScoreBall())
                {
                    GoToTop();
                    gameObject.SetActive(false);
                }
                else
                {
                    GoToTop();
                    gameObject.SetActive(false);
                }
            }
        }

        public void CheckBricksActivation()
        {
            int deactiveObjects = 0;

            for (int i = 0; i < m_bricks.Length; i++)
            {
                if (!m_bricks[i].gameObject.activeInHierarchy && !m_extraBalls[i].gameObject.activeInHierarchy)
                {
                    deactiveObjects++;
                }
            }

            if (deactiveObjects == m_bricks.Length)
            {
                gameObject.SetActive(false);
                GoToTop();
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

        public bool HasActiveScoreBall()
        {
            bool hasActiveScoreBall = false;

            for (int i = 0; i < m_extraBalls.Length; i++)
            {
                if (m_extraBalls[i].gameObject.activeInHierarchy)
                {
                    m_extraBalls[i].PlayParticle();
                    BallLauncherView.Instance.IncreaseBallsAmountFromOutSide(1);
                    hasActiveScoreBall = true;
                    break;
                }
            }

            return hasActiveScoreBall;
        }
    }
}