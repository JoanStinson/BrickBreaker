using TMPro;
using UnityEngine;

namespace JGM.Game
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private BrickRowSpawnerView m_brickRowSpawnerView;
        [SerializeField] private BallLauncherView m_ballLauncherView;
        [SerializeField] private TextMeshProUGUI m_scoreText;
        [SerializeField] private TextMeshProUGUI m_highScoreText;

        public void Initialize(GameView gameView)
        {
            var model = gameView.Model;
            m_highScoreText.text = model.HighScore.ToString();
            m_scoreText.text = model.Score.ToString();

            m_brickRowSpawnerView.Initialize(model);
            m_brickRowSpawnerView.OnBrickTouchedFloor += OnBrickTouchedFloor;
            m_brickRowSpawnerView.OnPickupExtraBall += OnPickupExtraBall;

            m_ballLauncherView.Initialize(model);
            m_ballLauncherView.OnBallsReturned += OnBallsReturned;
        }

        private void OnBrickTouchedFloor()
        {

        }

        private void OnPickupExtraBall()
        {
            m_ballLauncherView.AddExtraBall();
        }

        private void OnBallsReturned()
        {
            m_brickRowSpawnerView.MoveDownRows();
            m_brickRowSpawnerView.SpawnBricks();
        }
    }
}