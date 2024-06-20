using TMPro;
using UnityEngine;

namespace JGM.Game
{
    public class LeaderboardCellView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_text;

        public void SetText(string text)
        {
            m_text.text = text;
        }
    }
}
