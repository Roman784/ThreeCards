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
        private ToolsSettings _toolsSettings;
        private ChipsCounter _chipsCounter;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider, ChipsCounter chipsCounter)
        {
            _toolsSettings = settingsProvider.GameSettings.ToolsSettings;
            _chipsCounter = chipsCounter;
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

        public void Init(FieldShufflingService fieldShufflingService, MagicStickService magicStickService)
        {
            _fieldShufflingService = fieldShufflingService;
            _magicStickService = magicStickService;
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

            if (_chipsCounter.Count < _toolsSettings.FieldShufflingCost) return;
            _chipsCounter.Reduce(_toolsSettings.FieldShufflingCost);

            var onCompleted = _fieldShufflingService.Shuffle();
            DisableUntilComplete(onCompleted);
        }

        private void PickThree()
        {
            if (!_isEnabled) return;

            if (_chipsCounter.Count < _toolsSettings.MagicStickCost) return;
            _chipsCounter.Reduce(_toolsSettings.MagicStickCost);

            var onCompleted = _magicStickService.PickThree();
            DisableUntilComplete(onCompleted);
        }

        private void RestartLevel()
        {

        }

        private void DisableUntilComplete(Observable<Unit> onCompleted)
        {
            if (onCompleted != null)
            {
                Disable();
                onCompleted.Subscribe(_ => Enable());
            }
        }
    }
}
