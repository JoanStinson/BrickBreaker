using TMPro;
using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class LeaderboardCellView : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<LeaderboardCellView> { }

        [SerializeField]
        private TextMeshProUGUI m_text;

        public void SetText(string text)
        {
            m_text.text = text;
        }
    }
}
