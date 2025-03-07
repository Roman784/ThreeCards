using Currencies;
using GameplayServices;
using R3;
using Settings;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameplayTools
    {
        private GameplayToolsView _view;
        private bool _isEnabled;

        private FieldShufflingService _fieldShufflingService;
        private MagicStickService _magicStickService;
        private LevelRestarterService _levelRestarterService;
        private ToolsSettings _toolsSettings;
        private ChipsCounter _chipsCounter;

        private AdvertisingChipsPopUp _advertisingChipsPopUp;
        private AdvertisingChipsPopUp.Factory _advertisingChipsPopUpFactroy;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider, 
                               ChipsCounter chipsCounter, 
                               AdvertisingChipsPopUp.Factory advertistingChipsPopUpFactory)
        {
            _toolsSettings = settingsProvider.GameSettings.ToolsSettings;
            _chipsCounter = chipsCounter;
            _advertisingChipsPopUpFactroy = advertistingChipsPopUpFactory;
        }

        public void BindView(GameplayToolsView view)
        {
            _view = view;

            _view.Disable();
            _view.SetCosts(_toolsSettings.FieldShufflingCost, _toolsSettings.MagicStickCost);

            _view.OnShuffleField += () => ShuffleField();
            _view.OnPickThree += () => PickThree();
            _view.OnRestartLevel += () => RestartLevel();
        }

        public void Init(FieldShufflingService fieldShufflingService, 
                         MagicStickService magicStickService,
                         LevelRestarterService levelRestarterService)
        {
            _fieldShufflingService = fieldShufflingService;
            _magicStickService = magicStickService;
            _levelRestarterService = levelRestarterService;
        }

        public void Enable()
        {
            _isEnabled = true;
            _view.Enable();
        }

        public void Disable()
        {
            _isEnabled = false;
            _view.Disable();
        }

        private void ShuffleField()
        {
            if (!_isEnabled) return;

            if (!CheckCost(_toolsSettings.FieldShufflingCost)) return;
            _chipsCounter.Reduce(_toolsSettings.FieldShufflingCost);

            var onCompleted = _fieldShufflingService.Shuffle();
            DisableUntilComplete(onCompleted);

            _view.CreateShufflingPurchaseEffect();
        }

        private void PickThree()
        {
            if (!_isEnabled) return;

            if (!CheckCost(_toolsSettings.MagicStickCost)) return;
            _chipsCounter.Reduce(_toolsSettings.MagicStickCost);

            var onCompleted = _magicStickService.PickThree();
            DisableUntilComplete(onCompleted);

            _view.CreateMagicStickPurchaseEffect();
        }

        private void RestartLevel()
        {
            if (!_isEnabled) return;

            var onCompleted = _levelRestarterService.Restart();
            DisableUntilComplete(onCompleted);
        }

        private void DisableUntilComplete(Observable<Unit> onCompleted)
        {
            if (onCompleted != null)
            {
                Disable();
                onCompleted.Subscribe(_ => Enable());
            }
        }

        private bool CheckCost(int cost)
        {
            if (_chipsCounter.Count >= cost) return true;

            OpenAdvertisingChipsPopUp();
            return false;
        }

        private void OpenAdvertisingChipsPopUp()
        {
            if (_advertisingChipsPopUp == null)
                _advertisingChipsPopUp = _advertisingChipsPopUpFactroy.Create();

            _advertisingChipsPopUp.Open();
        }
    }
}
