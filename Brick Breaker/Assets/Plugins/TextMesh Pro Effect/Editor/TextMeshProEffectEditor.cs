using UnityEditor;
using UnityEngine;

namespace TMPro.Editor
{
    [CustomEditor(typeof(TextMeshProEffect))]
    public class TextMeshProEffectEditor : UnityEditor.Editor
    {
        public override bool RequiresConstantRepaint() => true;

        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
            base.OnInspectorGUI();
        }
    }
}