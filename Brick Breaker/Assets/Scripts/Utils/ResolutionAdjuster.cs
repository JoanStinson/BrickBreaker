using System.Collections;
using UnityEngine;

namespace JGM.Game
{
    public class ResolutionAdjuster : MonoBehaviour
    {
        [SerializeField] private Transform m_leftColliderTransform;
        [SerializeField] private Transform m_rightColliderTransform;
        [SerializeField] private Transform m_bricksParent;
        [SerializeField] private BrickRowSpawnerView brickRowSpawnerView;

        private bool differentResolution => (Screen.width != m_baseScreenSize.x || Screen.height != m_baseScreenSize.y);
        private readonly Vector2 m_baseScreenSize = new Vector2(1080f, 1920f);
        private readonly Vector2 m_baseBrickRowScale = new Vector2(0.9f, 1.3f);
        private const float m_baseLeftColliderX = -3.62f;
        private const float m_baseRightColliderX = 3.62f;
        private const float rowDistanceOffset = 0.1f;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            if (differentResolution)
            {
                AdjustColliderPositions();
                AdjustBrickSizes();
            }
        }

        private void AdjustColliderPositions()
        {
            float widthRatio = Screen.width / m_baseScreenSize.x;
            float heightRatio = Screen.height / m_baseScreenSize.y;
            float aspectRatio = widthRatio / heightRatio;
            float newLeftColliderX = m_baseLeftColliderX * aspectRatio;
            float newRightColliderX = m_baseRightColliderX * aspectRatio;
            AdjustColliderPosition(m_leftColliderTransform, newLeftColliderX);
            AdjustColliderPosition(m_rightColliderTransform, newRightColliderX);
        }

        private void AdjustColliderPosition(Transform colliderTransform, float newColliderX)
        {
            Vector3 newPosition = colliderTransform.position;
            newPosition.x = newColliderX;
            colliderTransform.localPosition = newPosition;
        }

        private void AdjustBrickSizes()
        {
            float previousAspectRatio = m_baseScreenSize.x / m_baseScreenSize.y;
            float currentAspectRatio = Screen.width / (float)Screen.height;

            for (int i = 0; i < m_bricksParent.childCount; i++)
            {
                float brickLocalScaleX = m_baseBrickRowScale.x / (previousAspectRatio / currentAspectRatio);
                float brickLocalScaleY = m_baseBrickRowScale.y / (previousAspectRatio / currentAspectRatio);
                var newScale = new Vector3(brickLocalScaleX, brickLocalScaleY);
                m_bricksParent.GetChild(i).localScale = newScale;
            }

            brickRowSpawnerView.SpawningRowDistance = currentAspectRatio + rowDistanceOffset;
        }
    }
}
