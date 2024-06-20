using System;
using TMPro;
using UnityEngine;

namespace JGM.Game
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private BrickRowSpawnerView m_brickRowSpawnerView;
        [SerializeField] private BallLauncherView m_ballLauncherView;
        [SerializeField] private TextMeshProUGUI m_highScoreText;
        [SerializeField] private TextMeshProUGUI m_scoreText;

        private GameView m_gameView;
        private GameModel m_gameModel;
        private int m_highScore;
        private int m_score;

        public void Initialize(GameView gameView)
        {
            m_gameView = gameView;
            m_gameModel = gameView.Model;
            m_highScore = m_gameModel.HighScore;
            m_highScoreText.text = $"High Score: {m_highScore.ToString()}";
            m_scoreText.text = $"Score: {m_score.ToString()}";

            m_brickRowSpawnerView.Initialize(m_gameModel);
            m_brickRowSpawnerView.OnBrickHit += OnBrickHit;
            m_brickRowSpawnerView.OnPickupExtraBall += OnPickupExtraBall;
            m_brickRowSpawnerView.OnBrickTouchedFloor += OnBrickTouchedFloor;

            m_ballLauncherView.Initialize(m_gameModel);
            m_ballLauncherView.OnBallsReturned += OnBallsReturned;
        }

        private void OnBrickHit()
        {
            m_score++;
            m_scoreText.text = $"Score: {m_score.ToString()}";
            m_gameModel.Score = m_score;

            if (m_score > m_highScore)
            {
                m_highScore = m_score;
                m_highScoreText.text = $"High Score: {m_highScore.ToString()}";
                m_gameModel.HighScore = m_highScore;
            }
        }

        private void OnPickupExtraBall()
        {
            m_ballLauncherView.AddExtraBall();
        }

        private void OnBrickTouchedFloor()
        {
            m_gameView.OnBrickTouchedFloor();
        }

        private void OnBallsReturned()
        {
            m_brickRowSpawnerView.MoveDownRows();
            m_brickRowSpawnerView.SpawnBricks();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}