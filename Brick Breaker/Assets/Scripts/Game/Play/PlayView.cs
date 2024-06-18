using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.Game
{
    public class PlayView : ScreenView
    {
        [SerializeField] private AnimatedText m_playerTurnText;
        [SerializeField] private BoardView m_boardView;
        [SerializeField] private PiecesSpawnView[] m_piecesSpawnViews;
        [SerializeField] private MessageView m_messageView;
        [SerializeField] private TutorialView m_tutorialView;
        [SerializeField] private Button m_backButton;
        [SerializeField] private float m_showWinnerSeconds = 1f;
        [Inject] private ICoroutineService m_coroutineService;

        private GameView m_gameView;
        private RectTransform m_canvasTransform;
        private PlayController m_playController;

        public override void Initialize(GameView gameView)
        {
            m_gameView = gameView;
            m_canvasTransform = (RectTransform)gameView.Canvas.transform;
            m_playController = new PlayController(m_boardView);

            var boardModel = m_playController.BuildBoardModel(gameView.Model);
            m_boardView.Initialize(m_gameView.Model, boardModel);
            m_boardView.OnPiecePlaced += OnPiecePlaced;
            InitializePieces();

            m_backButton.onClick.AddListener(OnClickBackButton);
        }

        private async void OnPiecePlaced()
        {
            if (!m_gameView.Model.CompletedTutorial)
            {
                m_tutorialView.HideTutorial();
                m_gameView.Model.CompletedTutorial = !m_gameView.Model.ShowTutorialAlways;
            }

            if (m_playController.TicTacToeFound())
            {
                await OnTicTacToeFound();
                return;
            }

            ChangePlayerTurn();
            CheckIfPlayerCanMove();
        }

        private async Task OnTicTacToeFound()
        {
            int playerWinId = m_playController.GetPlayerTurn();
            m_boardView.HighlightPlayerWinCells(playerWinId);
            foreach (var piecesSpawnView in m_piecesSpawnViews)
            {
                piecesSpawnView.EnableAllPiecesInteraction();
            }
            m_canvasGroup.blocksRaycasts = false;

            await Task.Delay(TimeSpan.FromSeconds(m_showWinnerSeconds));
            m_gameView.OnTicTacToeFound(playerWinId);
        }

        private void ChangePlayerTurn()
        {
            m_playController.ChangePlayerTurn();
            SetPlayerTurn();
        }

        private void SetPlayerTurn()
        {
            int playerTurn = m_playController.GetPlayerTurn();
            int nonPlayerTurn = m_playController.GetNonPlayerTurn();

            if (m_boardView.PiecesOnBoard > m_playController.GetMinBoardPiecesForTicTacToe())
            {
                m_piecesSpawnViews[playerTurn].EnableAllPiecesInteraction();
            }
            else
            {
                m_piecesSpawnViews[playerTurn].EnableNonPlacedPiecesInteraction();
            }

            m_playerTurnText.SetIntegerValue(playerTurn + 1);
            m_piecesSpawnViews[nonPlayerTurn].DisableAllPiecesInteraction();
        }

        private void CheckIfPlayerCanMove()
        {
            int playerTurn = m_playController.GetPlayerTurn();
            var playerPieces = m_piecesSpawnViews[playerTurn].GetPieces();

            if (!m_playController.CanPlayerMove(playerPieces))
            {
                m_messageView.ShowMessage(playerTurn + 1, ChangePlayerTurn);
            }
        }

        private void InitializePieces()
        {
            for (int i = 0; i < m_piecesSpawnViews.Length; i++)
            {
                m_piecesSpawnViews[i].Initialize(m_gameView.Model, i, m_boardView, m_canvasTransform);
            }
        }

        private void OnClickBackButton()
        {
            m_gameView.OnClickPlayBackButton();
        }

        public override void Show()
        {
            gameObject.SetActive(true);
            SetupNewGame();
            m_coroutineService.StartExternalCoroutine(DisableLayoutGroups());
            base.Show();
        }

        private void SetupNewGame()
        {
            m_boardView.ClearBoard();
            InitializePieces();
            m_playController.GenerateFirstTurnRandomly();
            SetPlayerTurn();
            m_messageView.HideMessage();
            m_tutorialView.HideTutorial();

            if (!m_gameView.Model.CompletedTutorial)
            {
                int playerTurnId = m_playController.GetPlayerTurn();
                m_tutorialView.ShowTutorial(playerTurnId);
            }
        }

        private IEnumerator DisableLayoutGroups()
        {
            yield return new WaitForEndOfFrame();

            foreach (var spawnView in m_piecesSpawnViews)
            {
                spawnView.DisableLayoutGroup();
            }
        }
    }
}
