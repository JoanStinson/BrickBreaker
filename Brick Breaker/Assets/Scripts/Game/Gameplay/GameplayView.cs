using DG.Tweening.Core.Easing;
using UnityEngine;

namespace JGM.Game
{
    public class GameplayView : MonoBehaviour
    {
        public void Initialize()
        {
            GameplayController.Instance.m_GameState = GameplayController.GameState.Playable;
        }
    }
}
