using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class BallLauncherView : MonoBehaviour
    {
        public static BallLauncherView Instance;

        public SpriteRenderer m_BallSprite;
        public int m_BallsAmount;
        public bool m_CanPlay = true;
        public int m_TempAmount = 0;

        [SerializeField] private GameObject m_deactivatableChildren;
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField] private Color m_correctLineColor;
        [SerializeField] private Color m_wrongLineColor;
        [SerializeField] private Text m_ballsAmountText;
        [SerializeField] private BallView m_ballViewPrefab;
        [SerializeField] private Button m_returnBallsButton;
        [SerializeField] private int m_startingBallsPoolAmount = 10;

        private List<BallView> m_ballInstances;
        private Vector3 m_startPosition;
        private Vector3 m_endPosition;
        private Vector3 m_worldPosition;
        private Vector3 m_direction;
        private Vector3 m_defaultStartPosition;

        private void Awake()
        {
            Instance = this;
            m_CanPlay = true;
            m_defaultStartPosition = transform.position;
            m_BallsAmount = PlayerPrefs.GetInt("balls", 1);
            m_ballInstances = new List<BallView>(m_startingBallsPoolAmount);
            m_returnBallsButton.onClick.AddListener(ReturnAllBallsToNewStartPosition);
        }

        private void Start()
        {
            SpawNewBall(m_startingBallsPoolAmount);
        }

        private void Update()
        {
            if (GameplayView.Instance.m_GameState == GameplayView.GameState.MainMenu || GameplayView.Instance.m_GameState == GameplayView.GameState.GameOver)
            {
                return;
            }

            if (!m_CanPlay)
            {
                return;
            }

            if (Time.timeScale != 0 && GameplayView.Instance.m_GameState != GameplayView.GameState.GameOver)
            {
                m_worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10;
            }

            if (Input.GetMouseButtonDown(0))
            {
                StartDrag(m_worldPosition);
            }
            else if (Input.GetMouseButton(0))
            {
                ContinueDrag(m_worldPosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }

        private void StartDrag(Vector3 worldPosition)
        {
            m_startPosition = worldPosition;
        }

        private void ContinueDrag(Vector3 worldPosition)
        {
            Vector3 tempEndposition = worldPosition;
            Vector3 tempDirection = tempEndposition - m_startPosition;
            tempDirection.Normalize();

            // Getting the angle in radians. you can replace 1.35f with any number or without hardcode like this
            if (Mathf.Abs(Mathf.Atan2(tempDirection.x, tempDirection.y)) < 1.35f)
            {
                m_lineRenderer.startColor = m_correctLineColor;
                m_lineRenderer.endColor = m_correctLineColor;
            }
            else
            {
                m_lineRenderer.startColor = m_wrongLineColor;
                m_lineRenderer.endColor = m_wrongLineColor;
            }

            m_endPosition = worldPosition;
            m_lineRenderer.SetPosition(1, m_endPosition - m_startPosition);
        }

        private void EndDrag()
        {
            if (m_startPosition == m_endPosition)
            {
                return;
            }

            m_direction = m_endPosition - m_startPosition;
            m_direction.Normalize();
            m_lineRenderer.SetPosition(1, Vector3.zero);

            //TODO hardcode for this time. fix it!
            if (Mathf.Abs(Mathf.Atan2(m_direction.x, m_direction.y)) < 1.35f)
            {
                if (m_ballInstances.Count < m_BallsAmount)
                {
                    SpawNewBall(m_BallsAmount - m_ballInstances.Count);
                }

                m_CanPlay = false;
                StartCoroutine(StartShootingBalls());
            }
        }

        public void OnMainMenuActions()
        {
            m_CanPlay = false;
            m_BallsAmount = 1;
            m_ballsAmountText.text = "x" + m_BallsAmount.ToString();
            m_BallSprite.enabled = true;
            m_deactivatableChildren.SetActive(true);
            transform.position = m_defaultStartPosition;
            m_BallSprite.transform.position = m_defaultStartPosition;
            ResetPositions();
            m_TempAmount = 0;
            BallView.ResetReturningBallsAmount();
            m_returnBallsButton.gameObject.SetActive(false);
            HideAllBalls();
        }

        public void ResetPositions()
        {
            m_startPosition = Vector3.zero;
            m_endPosition = Vector3.zero;
            m_worldPosition = Vector3.zero;
        }

        private void HideAllBalls()
        {
            foreach (var instance in m_ballInstances)
            {
                instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                instance.Disable();
            }
        }

        private void SpawNewBall(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                m_ballInstances.Add(Instantiate(m_ballViewPrefab, transform.parent, false));
                m_ballInstances[m_ballInstances.Count - 1].transform.localPosition = transform.localPosition;
                m_ballInstances[m_ballInstances.Count - 1].transform.localScale = transform.localScale;
                m_ballInstances[m_ballInstances.Count - 1].Disable();
            }
        }

        private IEnumerator StartShootingBalls()
        {
            m_returnBallsButton.gameObject.SetActive(true);
            m_BallSprite.enabled = false;

            int balls = m_BallsAmount;

            for (int i = 0; i < m_BallsAmount; i++)
            {
                if (m_CanPlay)
                {
                    break;
                }

                m_ballInstances[i].transform.position = transform.position;
                m_ballInstances[i].GetReadyAndAddForce(m_direction);
                balls--;
                m_ballsAmountText.text = "x" + balls.ToString();
                yield return new WaitForSeconds(0.05f);
            }

            if (balls <= 0)
            {
                m_deactivatableChildren.SetActive(false);
            }
        }

        public void ActivateHUD()
        {
            m_BallsAmount += m_TempAmount;

            // avoiding more balls than final brick level
            if (m_BallsAmount > BrickRowSpawnerView.Instance.m_LevelOfFinalBrick)
            {
                m_BallsAmount = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick;
            }

            m_TempAmount = 0;
            m_ballsAmountText.text = "x" + m_BallsAmount.ToString();
            m_deactivatableChildren.SetActive(true);
            m_returnBallsButton.gameObject.SetActive(false);
        }

        public void ReturnAllBallsToNewStartPosition()
        {
            if (BallView.FirstCollisionPoint != Vector3.zero)
            {
                transform.position = BallView.FirstCollisionPoint;
                BallView.ResetFirstCollisionPoint();
            }

            m_BallSprite.transform.position = transform.position;
            m_BallSprite.enabled = true;

            for (int i = 0; i < m_ballInstances.Count; i++)
            {
                m_ballInstances[i].DisablePhysics();
                m_ballInstances[i].MoveTo(transform.position, iTween.EaseType.easeInOutQuart, (Vector2.Distance(transform.position, m_ballInstances[i].transform.position) / 6.0f), "DeactiveSprite");
            }

            ResetPositions();
            BallView.ResetReturningBallsAmount();
            GameplayView.Instance.UpdateScore();
            BrickRowSpawnerView.Instance.MoveDownRows();
            BrickRowSpawnerView.Instance.SpawnNewRows();
            ActivateHUD();
            m_CanPlay = true;
        }

        public void IncreaseBallsAmountFromOutSide(int amout)
        {
            m_BallsAmount += amout;
            m_ballsAmountText.text = "x" + m_BallsAmount.ToString();
        }
    }
}