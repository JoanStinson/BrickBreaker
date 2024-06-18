using UnityEngine;
using static JGM.Game.LocalizationService;
using Random = UnityEngine.Random;

namespace JGM.Game
{
    public class GameController
    {
        private readonly IAudioService m_audioService;
        private readonly ILocalizationService m_localizationService;

        public GameController(IAudioService audioService, ILocalizationService localizationService)
        {
            m_audioService = audioService;
            m_localizationService = localizationService;
        }

        public GameModel BuildGameModel(GameSettings gameSettings)
        {
            return new GameModel(gameSettings);
        }

        public void PlayBackgroundMusic()
        {
            m_audioService.Play(AudioFileNames.BackgroundMusic, true);
        }

        public void PlayPressButtonSfx()
        {
            m_audioService.Play(AudioFileNames.PressButtonSfx);
        }

        public void ChangeLanguageToRandom()
        {
            Language currentLanguage = m_localizationService.CurrentLanguage;
            Language randomLanguage;

            do
            {
                randomLanguage = (Language)Random.Range(0, (int)Language.Count);
            }
            while (randomLanguage == currentLanguage);

            m_localizationService.SetLanguage(randomLanguage);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}