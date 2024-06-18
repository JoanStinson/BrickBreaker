using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace JGM.Game
{
    public class PieceView : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public class Factory : PlaceholderFactory<PieceView> { }

        public PieceModel Model { get; private set; }
        public CellView CellView { get; set; }

        [SerializeField]
        private Image m_image;

        private BoardView m_boardView;
        private RectTransform m_canvasTransform;
        private RectTransform m_transform;
        private Vector2 m_startingPosition;
        private Transform m_startingParent;

        public void Initialize(PieceModel pieceModel, BoardView boardView, RectTransform canvasTransform)
        {
            Model = pieceModel;
            m_boardView = boardView;
            m_canvasTransform = canvasTransform;
            m_image.sprite = Model.Sprite;
            m_transform = (RectTransform)transform;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_startingPosition = m_transform.anchoredPosition;
            m_startingParent = m_transform.parent;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_transform.SetParent(m_transform.root, true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetPiecePositionToMousePosition(eventData);
            m_boardView.HighlightCellIfValidForPiece(this, eventData);
        }

        private void SetPiecePositionToMousePosition(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvasTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
            m_transform.localPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            bool placePieceOnBoard = m_boardView.PlacePieceOnBoard(this, eventData);
            if (!placePieceOnBoard)
            {
                ReturnPieceToStartingPosition();
            }
        }

        private void ReturnPieceToStartingPosition()
        {
            m_transform.SetParent(m_startingParent, false);
            m_transform.anchoredPosition = m_startingPosition;
        }

        public void EnableInteraction()
        {
            m_image.raycastTarget = true;
            m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, Model.EnabledColorAlpha);
        }

        public void DisableInteraction()
        {
            m_image.raycastTarget = false;
            m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, Model.DisabledColorAlpha);
        }
    }
}
