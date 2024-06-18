using static JGM.Game.LocalizationService;

namespace JGM.Game
{
    public interface ILocalizationService
    {
        public Language CurrentLanguage { get; }
        public LanguageChangedEvent OnLanguageChanged { get; set; }

        void SetLanguage(Language language);
        string Localize(string textId);
        string GetFontNameForLanguage(Language language);
    }
}