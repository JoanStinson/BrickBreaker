using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace JGM.Game
{
    public class PiecesSpawnView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private VerticalLayoutGroup m_piecesSpawnParent;
        [Inject] private PieceView.Factory m_pieceViewFactory;
        [Inject] private ICoroutineService m_coroutineService;

        private readonly List<PieceView> m_pieceViewInstances = new List<PieceView>();

        public void Initialize(GameModel gameModel, int playerIndex, BoardView boardView, RectTransform canvasTransform)
        {
            InstantiatePieces(gameModel, playerIndex, boardView, canvasTransform);
            m_piecesSpawnParent.enabled = true;
        }

        private void InstantiatePieces(GameModel gameModel, int playerIndex, BoardView boardView, RectTransform canvasTransform)
        {
            m_piecesSpawnParent.transform.DestroyAllChildren();
            m_pieceViewInstances.Clear();

            foreach (var config in gameModel.GetPieceConfigs(playerIndex))
            {
                var pieceModel = new PieceModel(playerIndex, config.PieceType, config.Sprite, gameModel.PieceEnabledColorAlpha, gameModel.PieceDisabledColorAlpha);
                var pieceViewInstance = m_pieceViewFactory.Create();
                pieceViewInstance.transform.SetParent(m_piecesSpawnParent.transform, false);
                pieceViewInstance.Initialize(pieceModel, boardView, canvasTransform);
                pieceViewInstance.gameObject.SetName($"Player {playerIndex + 1} {config.PieceType}");
                m_pieceViewInstances.Add(pieceViewInstance);
            }
        }

        public void DisableLayoutGroup()
        {
            m_piecesSpawnParent.enabled = false;
        }

        public void EnableAllPiecesInteraction()
        {
            m_canvasGroup.blocksRaycasts = true;

            foreach (var instance in m_pieceViewInstances)
            {
                instance.EnableInteraction();
            }
        }

        public void DisableAllPiecesInteraction()
        {
            m_canvasGroup.blocksRaycasts = false;

            foreach (var instance in m_pieceViewInstances)
            {
                instance.DisableInteraction();
            }
        }

        public void EnableNonPlacedPiecesInteraction()
        {
            m_canvasGroup.blocksRaycasts = true;

            foreach (var instance in m_pieceViewInstances)
            {
                if (instance.CellView == null)
                {
                    instance.EnableInteraction();
                }
                else
                {
                    instance.DisableInteraction();
                }
            }
        }

        public IReadOnlyList<PieceView> GetPieces()
        {
            return m_pieceViewInstances;
        }
    }
}
