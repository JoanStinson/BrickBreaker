using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class BallLauncherView : MonoBehaviour
    {
        public Action OnBallsReturned { get; set; }
        public Vector3 FirstCollisionPoint { set; get; }

        public static BallLauncherView Instance;

        public SpriteRenderer m_BallSprite;
        public int m_ballsAmount;
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
        private int m_returnedBallsAmount;

        public void Initialize()
        {
            Instance = this;
            m_CanPlay = true;
            m_defaultStartPosition = transform.position;
            m_ballsAmount = PlayerPrefs.GetInt("balls", 1);
            m_ballsAmountText.text = "x" + m_ballsAmount.ToString();
            m_ballInstances = new List<BallView>(m_startingBallsPoolAmount);
            m_returnBallsButton.onClick.AddListener(ReturnAllBallsToNewStartPosition);
            SpawNewBall(m_startingBallsPoolAmount);
        }

        private void Update()
        {
            if (!m_CanPlay)
            {
                return;
            }

            if (Time.timeScale > 0)
            {
                m_worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -10;
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnBeginDrag(m_worldPosition);
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

        private void OnEndDrag()
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
                if (m_ballInstances.Count < m_ballsAmount)
                {
                    SpawNewBall(m_ballsAmount - m_ballInstances.Count);
                }

                m_CanPlay = false;
                StartCoroutine(StartShootingBalls());
            }
        }

        public void OnMainMenuActions()
        {
            m_CanPlay = false;
            m_ballsAmount = 1;
            m_ballsAmountText.text = "x" + m_ballsAmount.ToString();
            m_BallSprite.enabled = true;
            m_deactivatableChildren.SetActive(true);
            transform.position = m_defaultStartPosition;
            m_BallSprite.transform.position = m_defaultStartPosition;
            ResetPositions();
            m_TempAmount = 0;
            m_returnedBallsAmount = 0;
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
                BallView ballInstance = Instantiate(m_ballViewPrefab, transform.parent, false);
                ballInstance.OnBallReturned += OnReturnedBallInstance;
                m_ballInstances.Add(ballInstance);
                m_ballInstances[m_ballInstances.Count - 1].transform.localPosition = transform.localPosition;
                m_ballInstances[m_ballInstances.Count - 1].transform.localScale = transform.localScale;
                m_ballInstances[m_ballInstances.Count - 1].Disable();
            }
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
            if (FirstCollisionPoint != Vector3.zero)
            {
                transform.position = FirstCollisionPoint;
            }

            m_BallSprite.enabled = true;
            ActivateHUD();
            OnBallsReturned?.Invoke();
            BrickRowSpawnerView.Instance.MoveDownRows();
            BrickRowSpawnerView.Instance.SpawnBricks();
            FirstCollisionPoint = Vector3.zero;
            m_returnedBallsAmount = 0;
            m_CanPlay = true;
        }

        private IEnumerator StartShootingBalls()
        {
            m_returnBallsButton.gameObject.SetActive(true);
            m_BallSprite.enabled = false;

            int balls = m_ballsAmount;

            for (int i = 0; i < m_ballsAmount; i++)
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
            m_ballsAmount += m_TempAmount;

            // avoiding more balls than final brick level
            if (m_ballsAmount > BrickRowSpawnerView.Instance.m_LevelOfFinalBrick + 1)
            {
                m_ballsAmount = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick;
            }

            m_TempAmount = 0;
            m_ballsAmountText.text = "x" + m_ballsAmount.ToString();
            m_deactivatableChildren.SetActive(true);
            m_returnBallsButton.gameObject.SetActive(false);
        }

        public void ReturnAllBallsToNewStartPosition()
        {
            if (FirstCollisionPoint != Vector3.zero)
            {
                transform.position = FirstCollisionPoint;
                FirstCollisionPoint = Vector3.zero;
            }

            m_BallSprite.transform.position = transform.position;
            m_BallSprite.enabled = true;

            for (int i = 0; i < m_ballInstances.Count; i++)
            {
                m_ballInstances[i].DisablePhysics();
                float time = Vector2.Distance(transform.position, m_ballInstances[i].transform.position) / 6.0f;
                m_ballInstances[i].MoveTo(transform.position, iTween.EaseType.easeInOutQuart, time);
            }

            ResetPositions();
            m_returnedBallsAmount = 0;
            OnBallsReturned?.Invoke();
            BrickRowSpawnerView.Instance.MoveDownRows();
            BrickRowSpawnerView.Instance.SpawnBricks();
            ActivateHUD();
            m_CanPlay = true;
        }

        public void IncreaseBallsAmountFromOutSide(int amout)
        {
            m_ballsAmount += amout;
            m_ballsAmountText.text = "x" + m_ballsAmount.ToString();
        }

        public void AddExtraBall()
        {
            m_TempAmount++;
        }
    }
}