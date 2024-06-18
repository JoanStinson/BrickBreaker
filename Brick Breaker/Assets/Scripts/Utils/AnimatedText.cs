using DG.Tweening;
using UnityEngine;

namespace JGM.Game
{
    public class AnimatedText : MonoBehaviour
    {
        [SerializeField] private LocalizedText m_localizedText;
        [SerializeField] private DOTweenAnimation[] m_animations;

        public void SetIntegerValue(int value)
        {
            m_localizedText.SetIntegerValue(value);

            foreach (var animation in m_animations)
            {
                animation.DORewind();
                animation.DOPlay();
            }
        }
    }
}
