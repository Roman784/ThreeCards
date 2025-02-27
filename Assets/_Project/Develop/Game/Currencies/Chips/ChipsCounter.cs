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
        private int _chipsCount = new();
        private IGameStateProvider _gameStateProvider;

        private ChipsCounterView _view;

        public int Count => _chipsCount;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public void BindView(ChipsCounterView view)
        {
            _view = view;
        }

        public void InitChips(CardMatchingService cardMatchingService)
        {
            _chipsCount = _gameStateProvider.GameState.Chips.Value;
            _view?.SetCurrentCount(_chipsCount);

            cardMatchingService.OnCardsRemoved.Subscribe(removedCards =>
            {
                foreach (var card in removedCards)
                {
                    var chipsCount = CardMarkingMapper.GetRankValue(card.Rank);
                    Add(chipsCount, card.Position);
                }
            });
        }

        public void Reduce(int value)
        {
            if (value > _chipsCount)
                value = _chipsCount;

            _chipsCount -= value;
            _gameStateProvider.GameState.Chips.Value = _chipsCount;

            _view?.ChangeCounter(_chipsCount);
        }

        private void Add(int value, Vector3 initialColectionPosition)
        {
            Add(value, false);
            _view.AnimateCollection(value, initialColectionPosition).Subscribe(_ => 
            {
                _view?.ChangeCounter(_chipsCount);
            });
        }

        private void Add(int value, bool changeView = true)
        {
            _chipsCount += value;
            _gameStateProvider.GameState.Chips.Value = _chipsCount;

            if (changeView)
                _view?.SetCurrentCount(_chipsCount);
        }
    }
}
