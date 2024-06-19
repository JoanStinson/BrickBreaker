using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.Game
{
    public class BallLauncherView : MonoBehaviour
    {
        public Action OnBallsReturned { get; set; }

        [SerializeField] private Transform m_ballInstancesParent;
        [SerializeField] private SpriteRenderer m_ballSprite;
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField] private GameObject m_deactivatableChildren;
        [SerializeField] private Color m_correctLineColor;
        [SerializeField] private Color m_wrongLineColor;
        [SerializeField] private TextMeshProUGUI m_ballsAmountText;
        [SerializeField] private Button m_returnBallsButton;
        [SerializeField] private int m_startingBallsPoolAmount = 10;
        [SerializeField] private float m_dragAngle = 1.35f;
        [SerializeField] private float m_maxLineLength = 5.0f;
        [SerializeField] private GameObject m_handTutorial;
        [Inject] private BallView.Factory m_ballViewFactory;

        private GameModel m_gameModel;
        private List<BallView> m_ballInstances;
        private Vector3 m_startPosition;
        private Vector3 m_endPosition;
        private Vector3 m_worldPosition;
        private Vector3 m_direction;
        private Vector3 m_defaultStartPosition;
        private Vector3 m_firstCollisionPoint;
        private int m_ballsAmount;
        private int m_ballsAmountPendingToAdd;
        private int m_returnedBallsAmount;
        private bool m_canPlay;

        public void Initialize(GameModel gameModel)
        {
            m_gameModel = gameModel;
            m_canPlay = true;
            m_defaultStartPosition = transform.position;
            m_ballsAmount = PlayerPrefs.GetInt("balls", 1);
            m_ballsAmountText.text = "x" + m_ballsAmount.ToString();
            m_ballInstances = new List<BallView>(m_startingBallsPoolAmount);
            SpawNewBalls(m_startingBallsPoolAmount);
            m_returnBallsButton.onClick.AddListener(ReturnAllBallsToNewStartPosition);
            m_lineRenderer.positionCount = 2;
        }

        private void SpawNewBalls(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                BallView ballInstance = m_ballViewFactory.Create();
                ballInstance.transform.SetParent(m_ballInstancesParent, false);
                ballInstance.Initialize(this);
                ballInstance.OnBallReturned += OnReturnedBallInstance;
                m_ballInstances.Add(ballInstance);
                m_ballInstances[m_ballInstances.Count - 1].transform.localPosition = transform.localPosition;
                m_ballInstances[m_ballInstances.Count - 1].transform.localScale = transform.localScale;
                m_ballInstances[m_ballInstances.Count - 1].DisableBall();
            }
        }

        private void ReturnAllBallsToNewStartPosition()
        {
            if (m_firstCollisionPoint != Vector3.zero)
            {
                transform.position = m_firstCollisionPoint;
                m_firstCollisionPoint = Vector3.zero;
            }

            m_ballSprite.transform.position = transform.position;
            m_ballSprite.enabled = true;

            for (int i = 0; i < m_ballInstances.Count; i++)
            {
                m_ballInstances[i].DisableBallPhysics();
                float time = Vector2.Distance(transform.position, m_ballInstances[i].transform.position) / 6.0f;
                m_ballInstances[i].MoveBallTo(transform.position, iTween.EaseType.easeInOutQuart, time);
            }

            ResetPositions();
            m_returnedBallsAmount = 0;
            OnBallsReturned?.Invoke();
            ActivateHUD();
            m_canPlay = true;
        }

        private void Update()
        {
            if (!m_canPlay)
            {
                return;
            }

            if (Time.timeScale > 0)
            {
                m_worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10;
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnDrag(m_worldPosition);
            }
            else if (Input.GetMouseButton(0))
            {
                OnDrag(m_worldPosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnEndDrag();
            }
        }

        private void OnBeginDrag(Vector3 worldPosition)
        {
            m_startPosition = worldPosition;
        }

        private void OnDrag(Vector3 worldPosition)
        {
            Vector3 tempEndPosition = worldPosition;
            Vector3 tempDirection = tempEndPosition - m_defaultStartPosition;
            tempDirection.Normalize();

            if (Mathf.Abs(Mathf.Atan2(tempDirection.x, tempDirection.y)) < m_dragAngle)
            {
                m_lineRenderer.startColor = m_correctLineColor;
                m_lineRenderer.endColor = m_correctLineColor;
            }
            else
            {
                m_lineRenderer.startColor = m_wrongLineColor;
                m_lineRenderer.endColor = m_wrongLineColor;
            }

            Vector3 lineEndPosition = tempDirection * m_maxLineLength;
            m_lineRenderer.SetPosition(1, lineEndPosition);
            m_direction = tempDirection;
        }

        private void OnEndDrag()
        {
            m_lineRenderer.SetPosition(1, Vector3.zero);

            if (Mathf.Abs(Mathf.Atan2(m_direction.x, m_direction.y)) < m_dragAngle)
            {
                if (m_ballInstances.Count < m_ballsAmount)
                {
                    SpawNewBalls(m_ballsAmount - m_ballInstances.Count);
                }

                m_canPlay = false;
                StartCoroutine(StartShootingBalls());
            }
        }

        private IEnumerator StartShootingBalls()
        {
            m_handTutorial.SetActive(false);
            m_returnBallsButton.gameObject.SetActive(true);
            m_ballSprite.enabled = false;

            int balls = m_ballsAmount;

            for (int i = 0; i < m_ballsAmount; i++)
            {
                if (m_canPlay)
                {
                    break;
                }

                m_ballInstances[i].transform.position = transform.position;
                m_ballInstances[i].ShootBall(m_direction);
                balls--;
                m_ballsAmountText.text = "x" + balls.ToString();
                yield return new WaitForSeconds(0.05f);
            }

            if (balls <= 0)
            {
                m_deactivatableChildren.SetActive(false);
            }
        }

        public void ResetPositions()
        {
            m_startPosition = Vector3.zero;
            m_endPosition = Vector3.zero;
            m_worldPosition = Vector3.zero;
        }

        private void OnReturnedBallInstance(BallView ballView)
        {
            ballView.HideBall();
            m_returnedBallsAmount++;

            if (m_returnedBallsAmount == m_ballsAmount)
            {
                ContinuePlaying();
            }
        }

        private void ContinuePlaying()
        {
            if (m_firstCollisionPoint != Vector3.zero)
            {
                transform.position = m_firstCollisionPoint;
            }

            m_ballSprite.enabled = true;
            ActivateHUD();
            OnBallsReturned?.Invoke();
            m_firstCollisionPoint = Vector3.zero;
            m_returnedBallsAmount = 0;
            m_canPlay = true;
        }

        public void ActivateHUD()
        {
            m_ballsAmount += m_ballsAmountPendingToAdd;

            if (m_ballsAmount > m_gameModel.LevelOfFinalBrick + 1)
            {
                m_ballsAmount = m_gameModel.LevelOfFinalBrick;
            }

            m_ballsAmountPendingToAdd = 0;
            m_ballsAmountText.text = "x" + m_ballsAmount.ToString();
            m_deactivatableChildren.SetActive(true);
            m_returnBallsButton.gameObject.SetActive(false);
        }

        public void AddExtraBall()
        {
            m_ballsAmountPendingToAdd++;
        }

        public Vector3 UpdateFirstCollisionPoint()
        {
            if (m_firstCollisionPoint == Vector3.zero)
            {
                m_firstCollisionPoint = transform.position;
                m_ballSprite.transform.position = m_firstCollisionPoint;
                m_ballSprite.enabled = true;
            }

            return m_firstCollisionPoint;
        }
    }
}
