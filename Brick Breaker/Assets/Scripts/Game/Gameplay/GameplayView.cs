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

        public void Initialize(GameView gameView)
        {
            var model = gameView.Model;
            m_highScoreText.text = model.HighScore.ToString();
            m_scoreText.text = model.Score.ToString();

            m_brickRowSpawnerView.Initialize();
            m_brickRowSpawnerView.m_LevelOfFinalBrick = 1;
            m_brickRowSpawnerView.OnBrickTouchedFloor += OnBrickTouchedFloor;
            m_brickRowSpawnerView.OnPickupExtraBall += OnPickupExtraBall;

            m_ballLauncherView.Initialize();
            m_ballLauncherView.m_CanPlay = true;
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

        }
    }
}