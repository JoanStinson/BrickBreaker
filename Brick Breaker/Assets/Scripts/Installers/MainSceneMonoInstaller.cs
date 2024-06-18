using UnityEngine;
using Zenject;

namespace JGM.Game
{
    public class MainSceneMonoInstaller : MonoInstaller
    {
        [SerializeField] private AudioService m_audioServiceInstance;
        [SerializeField] private CoroutineService m_coroutineServiceInstance;

        public override void InstallBindings()
        {
            Container.Bind<IAudioService>().FromInstance(m_audioServiceInstance);
            Container.Bind<ICoroutineService>().FromInstance(m_coroutineServiceInstance);
            Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
            Container.Bind<IHapticFeedbackService>().To<BasicHapticFeedbackService>().AsSingle();
        }
    }
}