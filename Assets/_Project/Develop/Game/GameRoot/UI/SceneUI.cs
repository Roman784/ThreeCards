using Currencies;
using GameState;
using Settings;
using UnityEngine;
using Zenject;

namespace UI
{
    public class SceneUI : MonoBehaviour
    {
        protected ISettingsProvider _settingsProvider;
        protected IGameStateProvider _gameStateProvider;
        protected PopUpProvider _popUpProvider;
        protected ChipsCounter _chipsCounter;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider,
                               IGameStateProvider gameStateProvider,
                               ChipsCounter chipsCounter,
                               PopUpProvider popUpProvider)
        {
            _settingsProvider = settingsProvider;
            _gameStateProvider = gameStateProvider;
            _chipsCounter = chipsCounter;
            _popUpProvider = popUpProvider;
        }

        public void OpenSettings()
        {
            _popUpProvider.OpenSettingsPopUp();
        }
    }
}
