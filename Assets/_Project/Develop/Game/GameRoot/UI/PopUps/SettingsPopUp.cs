using Zenject;

namespace UI
{
    public class SettingsPopUp : PopUp
    {
        private GameRulesPopUp _gameRulesPopUp;
        private GameRulesPopUp.Factory _gameRulesPopUpFactory;

        [Inject]
        private void Construct(GameRulesPopUp.Factory gameRulesPopUpFactory)
        {
            _gameRulesPopUpFactory = gameRulesPopUpFactory;
        }

        public void OpenGameRules()
        {
            if (_gameRulesPopUp == null)
                _gameRulesPopUp = _gameRulesPopUpFactory.Create();

            _gameRulesPopUp.Open();
        }

        public class Factory : PopUpFactory<SettingsPopUp>
        {
        }
    }
}
