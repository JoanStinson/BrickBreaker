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

        private int m_currentScore;

        public void Initialize(GameView gameView)
        {
            var model = gameView.Model;
            m_highScoreText.text = $"High Score: {model.HighScore.ToString()}";
            m_scoreText.text = $"Score: {m_currentScore.ToString()}";

            m_brickRowSpawnerView.Initialize(model);
            m_brickRowSpawnerView.OnBrickHit += OnBrickHit;
            m_brickRowSpawnerView.OnPickupExtraBall += OnPickupExtraBall;
            m_brickRowSpawnerView.OnBrickTouchedFloor += OnBrickTouchedFloor;

            m_ballLauncherView.Initialize(model);
            m_ballLauncherView.OnBallsReturned += OnBallsReturned;
        }

        private void OnBrickHit()
        {
            m_currentScore++;
            m_scoreText.text = $"Score: {m_currentScore.ToString()}";
        }

        private void OnPickupExtraBall()
        {
            m_ballLauncherView.AddExtraBall();
        }

        private void OnBrickTouchedFloor()
        {

        }

        private void OnBallsReturned()
        {
            m_brickRowSpawnerView.MoveDownRows();
            m_brickRowSpawnerView.SpawnBricks();
        }
    }
}