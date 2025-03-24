using GameplayRoot;
using Zenject;

namespace UI
{
    public class PopUpProvider
    {
        private AdvertisingChipsPopUp.Factory _advertisingChipsPopUpFactory;
        private SettingsPopUp.Factory _settingsPopUpFactory;
        private GameRulesPopUp.Factory _gameRulesPopUpFactory;

        [Inject]
        private void Construct(AdvertisingChipsPopUp.Factory advertisingChipsPopUpFactory,
                               SettingsPopUp.Factory settingsPopUpFactory,
                               GameRulesPopUp.Factory gameRulesPopUpFactory)
        {
            _advertisingChipsPopUpFactory = advertisingChipsPopUpFactory;
            _settingsPopUpFactory = settingsPopUpFactory;
            _gameRulesPopUpFactory = gameRulesPopUpFactory;
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
    }
}
