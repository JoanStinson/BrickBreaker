using UnityEngine;
using Zenject;

namespace JGM.Game
{
    [CreateAssetMenu(fileName = "New Main Scene Scriptable Object Installer", menuName = "Installers/Main Scene Scriptable Object Installer")]
    public class MainSceneScriptableObjectInstaller : ScriptableObjectInstaller<MainSceneScriptableObjectInstaller>
    {
        [SerializeField] private AudioLibrary m_audioLibraryInstance;
        [SerializeField] private FontLibrary m_fontLibraryInstance;

        public override void InstallBindings()
        {
            Container.Bind<AudioLibrary>().FromInstance(m_audioLibraryInstance);
            Container.Bind<FontLibrary>().FromInstance(m_fontLibraryInstance);
        }
    }
}