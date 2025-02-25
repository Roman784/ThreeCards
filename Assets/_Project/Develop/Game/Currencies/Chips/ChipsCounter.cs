using GameState;
using Zenject;
using R3;
using UnityEngine;
using GameplayServices;
using Gameplay;

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

        public void InitChips(CardMatchingService cardMatchingService)
        {
            _chipsCount.Value = _gameStateProvider.GameState.Chips.Value;
            _view?.SetCurrentCount(_chipsCount.Value);

            cardMatchingService.OnCardsRemoved.Subscribe(removedCards =>
            {
                foreach (var card in removedCards)
                {
                    var chipsCount = CardMarkingMapper.GetRankValue(card.Rank);
                    Add(chipsCount, card.Position);
                }
            });
        }

        private void Add(int value, Vector3 initialColectionPosition)
        {
            Add(value);
            _view.AnimateCollection(value, initialColectionPosition);
        }

        private void Add(int value)
        {
            _chipsCount.Value += value;
            _gameStateProvider.GameState.Chips.Value = _chipsCount.Value;
        }
    }
}
