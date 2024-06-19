using System.Collections;
using UnityEngine;

namespace JGM.Game
{
    public class ResolutionAdjuster : MonoBehaviour
    {
        [SerializeField] private Transform m_leftColliderTransform;
        [SerializeField] private Transform m_rightColliderTransform;
        [SerializeField] private Transform m_bricksParent;

        private bool resolutionDifferentFromBase => (Screen.width != baseScreenWidth || Screen.height != baseScreenHeight);
        private const float baseScreenWidth = 1080f;
        private const float baseScreenHeight = 1920f;
        private const float baseLeftColliderX = -3.62f;
        private const float baseRightColliderX = 3.62f;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            if (resolutionDifferentFromBase)
            {
                AdjustColliderPositions();
            }
        }

        private void AdjustColliderPositions()
        {
            float widthRatio = Screen.width / baseScreenWidth;
            float heightRatio = Screen.height / baseScreenHeight;
            float aspectRatio = widthRatio / heightRatio;
            float newLeftColliderX = baseLeftColliderX * aspectRatio;
            float newRightColliderX = baseRightColliderX * aspectRatio;
            AdjustColliderPosition(m_leftColliderTransform, newLeftColliderX);
            AdjustColliderPosition(m_rightColliderTransform, newRightColliderX);
        }

        private void AdjustColliderPosition(Transform colliderTransform, float newColliderX)
        {
            Vector3 newPosition = colliderTransform.position;
            newPosition.x = newColliderX;
            colliderTransform.localPosition = newPosition;
        }
    }
}
