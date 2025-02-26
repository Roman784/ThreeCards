using DG.Tweening;
using Gameplay;
using R3;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace GameplayServices
{
    public class MagicStickService
    {
        private Card[,] _cardsMap;
        private SlotBar _slotBar;
        private CardMatchingService _cardMatchingService;
        private CardLayoutService _cardLayoutService;

        private Subject<Unit> _cardsPickedSubj = new();

        public MagicStickService(Card[,] cardsMap, SlotBar slotBar, 
                                 CardMatchingService cardMatchingService, CardLayoutService cardLayoutService)
        {
            _cardsMap = cardsMap;
            _slotBar = slotBar;
            _cardMatchingService = cardMatchingService;
            _cardLayoutService = cardLayoutService;

            _cardMatchingService.OnCardsRemoved.Subscribe(_ => ShiftCards());
        }

        public Observable<Unit> PickThree()
        {
            var originCard = GetOriginCard();
            var cards = new List<Card>();
            cards.Capacity = 3;
            cards.Add(originCard);

            foreach (var card in _cardsMap)
            {
                if (card == null) continue;
                if (card.Suit != originCard.Suit) continue;
                if (card.IsDestroyed) continue;
                if (card == originCard) continue;

                cards.Add(card);

                if (cards.Count == 3) break;
            }

            _cardMatchingService.RemoveCards(cards);

            return _cardsPickedSubj;
        }

        private Card GetOriginCard()
        {
            foreach (var slot in _slotBar.Slots)
            {
                if (slot.HasCard)
                    return slot.Card;
            }

            var cards = new List<Card>();
            foreach (var card in _cardsMap)
            {
                if (card == null || card.IsDestroyed) continue;
                cards.Add(card);
            }

            return Randomizer.GetRandomValue(cards);
        }

        private void ShiftCards()
        {
            for (int columnI = 0; columnI < _cardsMap.GetLength(0); columnI++)
            {
                for (int cardI = 0; cardI < _cardsMap.GetLength(1); cardI++)
                {
                    var card = _cardsMap[columnI, cardI];
                    if (card == null || card.IsDestroyed)
                    {
                        var nextCard = GetNextCard(columnI, cardI);

                        if (nextCard == null) break;

                        MoveCard(nextCard, new Vector2Int(columnI, cardI)).Subscribe(_ =>
                        {
                            _cardsPickedSubj.OnNext(Unit.Default);
                        });
                    }
                }
            }
        }

        private Card GetNextCard(int columnI, int currentCardI)
        {
            for (int cardI = currentCardI + 1; cardI < _cardsMap.GetLength(1); cardI++)
            {
                var card = _cardsMap[columnI, cardI];

                if (card == null || card.IsDestroyed || card.IsPlaced) continue;
                return card;
            }
            return null;
        }

        private Observable<Unit> MoveCard(Card card, Vector2Int coordinates)
        {
            Vector2 position = _cardLayoutService.GetCardPosition(coordinates);

            _cardsMap[card.Coordinates.x, card.Coordinates.y] = null;
            _cardsMap[coordinates.x, coordinates.y] = card;

            card.SetCoordinates(coordinates);
            return card.Move(position, Ease.OutBounce, moveDuration: 0.5f);
        }
    }
}
