using R3;

namespace Localization
{
    public interface ILocalizationProvider
    {
        public Observable<Unit> OnLanguageChanged { get; }

        public Observable<Unit> LoadTranslations(string language);
        public string GetTranslation(string key);
        public void ChangeLanguage(string language);
    }
}
