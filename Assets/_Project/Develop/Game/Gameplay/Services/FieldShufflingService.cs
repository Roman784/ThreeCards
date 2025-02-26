using DG.Tweening;
using Gameplay;
using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace GameplayServices
{
    public class FieldShufflingService
    {
        private List<Card> _cards;
        private bool _isShuffling;

        private Card[,] _cardsMap;
        private SlotBar _slotBar;
        private CardFlippingService _cardFlippingService;

        private Coroutine _cardsReplacingRoutine;

        public FieldShufflingService(Card[,] cardsMap, SlotBar slotBar, CardFlippingService cardFlippingService)
        {
            _cardsMap = cardsMap;
            _slotBar = slotBar;
            _cardFlippingService = cardFlippingService;
        }

        public Observable<Unit> Shuffle()
        {
            var completedSubj = new Subject<Unit>();

            if (_isShuffling) return null;
            _isShuffling = true;

            SetCards();
            _cardFlippingService.CloseFirstCards()?.Subscribe(_ =>
            {
                var cardsReplacedSubj = new Subject<Unit>();
                cardsReplacedSubj.Subscribe(_ =>
                {
                    _cardFlippingService.OpenFirstCards();
                    _isShuffling = false;

                    completedSubj.OnNext(Unit.Default);
                    completedSubj.OnCompleted();
                });

                if (_cardsReplacingRoutine != null) Coroutines.StopRoutine(_cardsReplacingRoutine);
                _cardsReplacingRoutine = Coroutines.StartRoutine(ReplaceCardsRoutine(cardsReplacedSubj));
            });

            return completedSubj;
        }

        private void SetCards()
        {
            _cards = new();
            foreach (var card in _cardsMap)
            {
                if (card != null && !_slotBar.ContainsCard(card) && !card.IsDestroyed)
                    _cards.Add(card);
            }
        }

        private IEnumerator ReplaceCardsRoutine(Subject<Unit> cardsReplacedSubj)
        {
            bool canToNext = false;

            for (int i = 0; i < (float)_cards.Count / 1.5f; i++)
            {
                Card card1 = _cards[i];
                Card card2 = _cards[Random.Range(0, _cards.Count)];

                ReplaceCards(card1, card2).Subscribe(_ => canToNext = true);

                yield return new WaitUntil(() => canToNext);
                canToNext = false;
            }

            cardsReplacedSubj.OnNext(Unit.Default);
            cardsReplacedSubj.OnCompleted();
        }

        private Observable<Unit> ReplaceCards(Card card1, Card card2)
        {
            var coordinates = card1.Coordinates;
            var position = card1.Position;

            _cardsMap[card2.Coordinates.x, card2.Coordinates.y] = card1;
            _cardsMap[coordinates.x, coordinates.y] = card2;

            card1.SetCoordinates(card2.Coordinates);
            card1.Move(card2.Position, Ease.Flash, moveDuration: 0.1f);

            card2.SetCoordinates(coordinates);
            return card2.Move(position, Ease.Flash, moveDuration: 0.1f);
        }
    }
}
