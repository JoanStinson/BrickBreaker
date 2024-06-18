using System.Text;
using TMPro;
using UnityEngine;

namespace JGM.Game
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_fpsText;
        [SerializeField, Range(0, 10)] private float m_updateInterval = 1.0f;

        private readonly StringBuilder m_fpsStringBuilder = new StringBuilder(10);
        private float m_deltaTime = 0.0f;
        private float m_elapsedTime = 0.0f;
        private int m_frameCount = 0;

        private void Update()
        {
            m_deltaTime += Time.deltaTime;
            m_frameCount++;
            m_elapsedTime += Time.deltaTime;

            if (m_elapsedTime >= m_updateInterval)
            {
                CalculateFps();

                m_deltaTime = 0.0f;
                m_frameCount = 0;
                m_elapsedTime = 0.0f;
            }
        }

        private void CalculateFps()
        {
            float fps = m_frameCount / m_deltaTime;
            m_fpsStringBuilder.Length = 0;
            m_fpsStringBuilder.AppendFormat("{0:0.} FPS", fps);
            m_fpsText.text = m_fpsStringBuilder.ToString();
        }
    }
}
