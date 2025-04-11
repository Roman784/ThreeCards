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

        [Inject]
        private void Construct(AdvertisingChipsPopUp.Factory advertisingChipsPopUpFactory,
                               SettingsPopUp.Factory settingsPopUpFactory,
                               GameRulesPopUp.Factory gameRulesPopUpFactory,
                               LanguagePopUp.Factory languagePopUpFactory)
        {
            _advertisingChipsPopUpFactory = advertisingChipsPopUpFactory;
            _settingsPopUpFactory = settingsPopUpFactory;
            _gameRulesPopUpFactory = gameRulesPopUpFactory;
            _languagePopUpFactory = languagePopUpFactory;
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
    }
}
