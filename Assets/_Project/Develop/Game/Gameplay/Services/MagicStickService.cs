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
        private FieldService _fieldService;
        private CardMatchingService _cardMatchingService;
        private CardLayoutService _cardLayoutService;

        private Subject<Unit> _cardsPickedSubj = new();

        public MagicStickService(FieldService fieldService,
                                 CardMatchingService cardMatchingService, CardLayoutService cardLayoutService)
        {
            _fieldService = fieldService;
            _cardMatchingService = cardMatchingService;
            _cardLayoutService = cardLayoutService;
        }

        public Observable<Unit> PickThree()
        {
            var originCard = GetOriginCard();
            var cards = new List<Card>();
            cards.Capacity = 3;
            cards.Add(originCard);

            foreach (var card in _fieldService.Cards)
            {
                if (card == null) continue;
                if (card.Suit != originCard.Suit) continue;
                if (card.IsDestroyed) continue;
                if (card.IsBomb) continue;
                if (card == originCard) continue;

                cards.Add(card);

                if (cards.Count == 3) break;
            }

            _cardMatchingService.RemoveCards(cards).Subscribe(_ => ShiftCards());

            return _cardsPickedSubj;
        }

        private Card GetOriginCard()
        {
            foreach (var slot in _fieldService.SlotBar.Slots)
            {
                if (slot.HasCard)
                    return slot.Card;
            }

            var cards = new List<Card>();
            foreach (var card in _fieldService.Cards)
            {
                if (card == null || card.IsDestroyed || card.IsBomb) continue;
                cards.Add(card);
            }

            return Randomizer.GetRandomValue(cards);
        }

        private void ShiftCards()
        {
            Observable<Unit> onCompleted = null;

            for (int columnI = 0; columnI < _fieldService.HorizontalLength; columnI++)
            {
                for (int cardI = 0; cardI < _fieldService.VerticalLength; cardI++)
                {
                    var card = _fieldService.GetCard(columnI, cardI);
                    if (!_fieldService.IsCardExist(card))
                    {
                        var nextCard = GetNextCard(columnI, cardI);
                        if (nextCard == null) break;

                        onCompleted = MoveCard(nextCard, new Vector2Int(columnI, cardI));
                    }
                }
            }

            if (onCompleted == null)
                _cardsPickedSubj.OnNext(Unit.Default);
            else
                onCompleted.Subscribe(_ =>
                {
                    _cardsPickedSubj.OnNext(Unit.Default);
                });
        }

        private Card GetNextCard(int columnI, int currentCardI)
        {
            for (int cardI = currentCardI + 1; cardI < _fieldService.VerticalLength; cardI++)
            {
                var card = _fieldService.GetCard(columnI, cardI);

                if (card == null || card.IsDestroyed || card.IsPlaced) continue;
                return card;
            }
            return null;
        }

        private Observable<Unit> MoveCard(Card card, Vector2Int coordinates)
        {
            Vector2 position = _cardLayoutService.GetCardPosition(coordinates);

            _fieldService.SetCard(null, card.Coordinates);
            _fieldService.SetCard(card, coordinates);

            card.SetCoordinates(coordinates);
            return card.Move(position, Ease.OutBounce, moveDuration: 0.5f);
        }
    }
}
