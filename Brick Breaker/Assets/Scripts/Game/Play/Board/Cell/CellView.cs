using UnityEngine;
using UnityEngine.UI;

namespace JGM.Game
{
    public class CellView : MonoBehaviour
    {
        public CellModel Model => m_cellModel;

        [SerializeField]
        private Image m_image;

        private CellModel m_cellModel;

        public void Initialize(CellModel cellModel, int row, int column)
        {
            m_cellModel = cellModel;
            SetDefaultColor();
        }

        public void SetDefaultColor()
        {
            m_image.color = m_cellModel.DefaultColor;
        }

        public void SetHighlightedColor()
        {
            m_image.color = m_cellModel.HighlightedColor;
        }

        public void SetPiece(PieceView pieceView)
        {
            pieceView.CellView = this;
            pieceView.transform.SetParent(transform, false);
            pieceView.transform.localPosition = Vector3.zero;
            m_cellModel.SetPieceModel(pieceView.Model);
        }

        public void RemovePiece()
        {
            m_cellModel.RemovePieceModel();
        }

        public void DestroyPiece()
        {
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }
}
