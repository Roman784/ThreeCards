using GameState;
using Zenject;
using R3;
using UnityEngine;

namespace Currencies
{
    public class ChipsCounter
    {
        private ReactiveProperty<int> _chipsCount = new();
        private IGameStateProvider _gameStateProvider;

        private ChipsCounterView _view;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public void BindView(ChipsCounterView view)
        {
            _view = view;
            _chipsCount.Subscribe(value => _view.IncreaseCounter(_chipsCount.Value));
        }

        public void LoadChips()
        {
            _chipsCount.Value = _gameStateProvider.GameState.Chips.Value;
            _view?.SetCurrentCount(_chipsCount.Value);
        }

        public void Add(int value)
        {
            _chipsCount.Value += value;
            _gameStateProvider.GameState.Chips.Value = _chipsCount.Value;
        }

        public void Add(int value, Vector3 initialColectionPosition)
        {
            Add(value);
            _view.AnimateCollection(value, initialColectionPosition);
        }
    }
}
