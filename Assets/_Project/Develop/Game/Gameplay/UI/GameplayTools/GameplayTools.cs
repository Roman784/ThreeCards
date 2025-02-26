using GameplayServices;
using R3;
using UnityEngine;

namespace UI
{
    public class GameplayTools
    {
        private GameplayToolsView _view;
        private bool _isEnabled;

        private FieldShufflingService _fieldShufflingService;
        private MagicStickService _magicStickService;

        public void BindView(GameplayToolsView view)
        {
            _view = view;
            _view.Disable();

            _view.OnShuffleField += () => ShuffleField();
            _view.OnPickThree += () => PickThree();
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

            var onCompleted = _fieldShufflingService.Shuffle();
            DisableUntilComplete(onCompleted);
        }

        private void PickThree()
        {
            if (!_isEnabled) return;

            var onCompleted = _magicStickService.PickThree();
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
