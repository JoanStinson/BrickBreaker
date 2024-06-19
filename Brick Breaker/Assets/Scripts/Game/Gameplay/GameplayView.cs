using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private BrickRowSpawnerView m_brickRowSpawnerView;
        [SerializeField] private BallLauncherView m_ballLauncherView;
        [SerializeField] private Text m_scoreText;
        [SerializeField] private Text m_highScoreText;

        private int m_highScore;
        private int m_rings;

        public void Initialize()
        {
            m_highScore = PlayerPrefs.GetInt("best_score", 0);
            m_highScoreText.text = m_highScore.ToString();
            m_scoreText.text = $"0";

            m_brickRowSpawnerView.Initialize();
            m_brickRowSpawnerView.m_LevelOfFinalBrick = 1;
            m_brickRowSpawnerView.OnBrickRowTouchedFloor += OnBrickRowTouchedFloor;

            m_ballLauncherView.Initialize();
            m_ballLauncherView.m_CanPlay = true;
            m_ballLauncherView.OnReturnBallsToNewStartPosition += OnReturnBallsToNewStartPosition;
        }

        private void OnBrickRowTouchedFloor()
        {
            
        }

        private void OnReturnBallsToNewStartPosition()
        {
            UpdateScore();
        }

        public void UpdateScore()
        {
            if (BrickRowSpawnerView.Instance.m_LevelOfFinalBrick > m_highScore)
            {
                m_highScore = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick;
                m_highScoreText.text = m_highScore.ToString();
                PlayerPrefs.SetInt("best_score", m_highScore);
            }

            m_scoreText.text = BrickRowSpawnerView.Instance.m_LevelOfFinalBrick.ToString();
        }

        public void AddRingToInventory(int value)
        {
            if (value > 0)
            {
                m_rings += value;
            }

            PlayerPrefs.SetInt("rings", m_rings);
        }
    }
}