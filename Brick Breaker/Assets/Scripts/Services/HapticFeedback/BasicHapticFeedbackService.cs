using UnityEngine;

namespace JGM.Game
{
    public class BasicHapticFeedbackService : IHapticFeedbackService
    {
        public void TriggerVibration()
        {
#if !UNITY_EDITOR
            Handheld.Vibrate();
#endif
        }
    }
}
