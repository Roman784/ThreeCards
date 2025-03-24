using GameState;
using Zenject;
using R3;
using UnityEngine;
using GameplayServices;
using Gameplay;
using System.Collections.Generic;
using UI;

namespace Currencies
{
    public class ChipsCounter
    {
        private int _chipsCount;
        private ChipsCounterView _view;

        private IGameStateProvider _gameStateProvider;
        private PopUpProvider _popUpProvider;

        public int Count => _chipsCount;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider, PopUpProvider popUpProvider)
        {
            _gameStateProvider = gameStateProvider;
            _popUpProvider = popUpProvider;
        }

        public void BindView(ChipsCounterView view)
        {
            _view = view;

            _view.OnGetAdvertisingChips += () => _popUpProvider.OpenAdvertisingChipsPopUp();
        }

        public void InitChips(Observable<List<CardMatchingService.RemovedCard>> onCardsRemoved = null)
        {
            _chipsCount = _gameStateProvider.GameState.Chips.Value;
            _view?.SetCurrentCount(_chipsCount);

            onCardsRemoved?.Subscribe(removedCards =>
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

        public void Add(int value, bool changeView = true, bool instantly = true)
        {
            _chipsCount += value;
            _gameStateProvider.GameState.Chips.Value = _chipsCount;

            if (changeView)
                if (instantly)
                    _view?.SetCurrentCount(_chipsCount);
                else
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
    }
}
