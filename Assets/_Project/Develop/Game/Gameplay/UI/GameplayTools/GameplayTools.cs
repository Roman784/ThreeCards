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
        private PopUpProvider _popUpProvider;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider, 
                               ChipsCounter chipsCounter, 
                               PopUpProvider popUpProvider)
        {
            _toolsSettings = settingsProvider.GameSettings.ToolsSettings;
            _chipsCounter = chipsCounter;
            _popUpProvider = popUpProvider;
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

        public bool ShuffleField()
        {
            if (!_isEnabled) return false;

            if (!CheckCost(_toolsSettings.FieldShufflingCost)) return false;
            _chipsCounter.Reduce(_toolsSettings.FieldShufflingCost);

            var onCompleted = _fieldShufflingService.Shuffle();
            DisableUntilComplete(onCompleted);

            _view.CreateShufflingPurchaseEffect();

            return true;
        }

        public bool PickThree()
        {
            if (!_isEnabled) return false;

            if (!CheckCost(_toolsSettings.MagicStickCost)) return false;
            _chipsCounter.Reduce(_toolsSettings.MagicStickCost);

            var onCompleted = _magicStickService.PickThree();
            DisableUntilComplete(onCompleted.AsUnitObservable());

            onCompleted.Subscribe(result =>
            {
                if (!result)
                    _chipsCounter.Add(_toolsSettings.MagicStickCost, instantly:false);
            });

            _view.CreateMagicStickPurchaseEffect();

            return true;
        }

        public bool RestartLevel()
        {
            if (!_isEnabled) return false;

            var onCompleted = _levelRestarterService.Restart();
            DisableUntilComplete(onCompleted);

            return true;
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

            _popUpProvider.OpenAdvertisingChipsPopUp();
            return false;
        }
    }
}
