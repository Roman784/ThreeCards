using Zenject;

namespace UI
{
    public class SettingsPopUp : PopUp
    {
        private PopUpProvider _popUpProvider;

        [Inject]
        private void Construct(PopUpProvider popUpProvider)
        {
            _popUpProvider = popUpProvider;
        }

        public void OpenGameRules()
        {
            PlayButtonClickSound();
            _popUpProvider.OpenGameRulesPopUp();
        }

        public class Factory : PopUpFactory<SettingsPopUp>
        {
        }
    }
}
