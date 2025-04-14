using GameplayRoot;
using Zenject;

namespace UI
{
    public class PopUpProvider
    {
        private AdvertisingChipsPopUp.Factory _advertisingChipsPopUpFactory;
        private SettingsPopUp.Factory _settingsPopUpFactory;
        private GameRulesPopUp.Factory _gameRulesPopUpFactory;
        private LanguagePopUp.Factory _languagePopUpFactory;
        private AdvertisingErrorPopUp.Factory _advertisingErrorPopUpFactory;

        [Inject]
        private void Construct(AdvertisingChipsPopUp.Factory advertisingChipsPopUpFactory,
                               SettingsPopUp.Factory settingsPopUpFactory,
                               GameRulesPopUp.Factory gameRulesPopUpFactory,
                               LanguagePopUp.Factory languagePopUpFactory,
                               AdvertisingErrorPopUp.Factory advertisingErrorPopUpFactory)
        {
            _advertisingChipsPopUpFactory = advertisingChipsPopUpFactory;
            _settingsPopUpFactory = settingsPopUpFactory;
            _gameRulesPopUpFactory = gameRulesPopUpFactory;
            _languagePopUpFactory = languagePopUpFactory;
            _advertisingErrorPopUpFactory = advertisingErrorPopUpFactory;
        }

        public void OpenAdvertisingChipsPopUp()
        {
            _advertisingChipsPopUpFactory.Create().Open();
        }

        public void OpenGameRulesPopUp()
        {
            _gameRulesPopUpFactory.Create().Open();
        }

        public void OpenSettingsPopUp()
        {
            _settingsPopUpFactory.Create().Open();
        }

        public void OpenLanguagePopUp()
        {
            _languagePopUpFactory.Create().Open();
        }

        public void OpenAdvertisingErrorPopUp()
        {
            _advertisingErrorPopUpFactory.Create().Open();
        }
    }
}
