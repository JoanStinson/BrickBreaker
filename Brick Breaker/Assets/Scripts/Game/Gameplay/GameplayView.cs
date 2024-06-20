using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField] private BrickRowSpawnerView m_brickRowSpawnerView;
        [SerializeField] private BallLauncherView m_ballLauncherView;
        [SerializeField] private LocalizedText m_highScoreText;
        [SerializeField] private LocalizedText m_scoreText;
        [SerializeField] private GameObject m_gameplayCanvas;
        [Inject] private IAudioService m_audioService;

        private GameView m_gameView;
        private GameModel m_gameModel;
        private int m_highScore;
        private int m_score;

        public void Initialize(GameView gameView)
        {
            m_gameView = gameView;
            m_gameModel = gameView.Model;
            m_highScore = m_gameModel.HighScore;
            m_highScoreText.SetIntegerValue(m_highScore);
            m_scoreText.SetIntegerValue(m_score);

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
            m_scoreText.SetIntegerValue(m_score);
            m_gameModel.Score = m_score;

            if (m_score > m_highScore)
            {
                m_highScore = m_score;
                m_highScoreText.SetIntegerValue(m_highScore);
                m_gameModel.HighScore = m_highScore;
            }

            m_audioService.Play(AudioFileNames.HitSfx);
        }

        private void OnPickupExtraBall()
        {
            m_ballLauncherView.AddExtraBall();
            m_audioService.Play(AudioFileNames.ExtraBallSfx);
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
            m_gameplayCanvas.SetActive(true);
            m_score = 0;
            m_gameModel.Score = m_score;
            m_scoreText.SetIntegerValue(m_score);
            m_gameModel.LevelOfFinalBrick = 1;
            m_brickRowSpawnerView.Initialize(m_gameModel);
            m_ballLauncherView.Reset();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            m_gameplayCanvas.SetActive(false);
        }
    }
}