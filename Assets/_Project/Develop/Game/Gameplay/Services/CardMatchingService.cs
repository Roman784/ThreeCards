using Gameplay;
using System.Collections.Generic;
using R3;
using UnityEngine;
using DG.Tweening;

namespace GameplayServices
{
    public class CardMatchingService
    {
        private SlotBar _slotBar;

        private Subject<List<RemovedCard>> _cardRemovedSubj = new();
        public Observable<List<RemovedCard>> OnCardsRemoved => _cardRemovedSubj;

        public CardMatchingService(SlotBar slotBar, Observable<Card> onCardPlaced)
        {
            _slotBar = slotBar;

            onCardPlaced.Subscribe(_ => Match());
            if (_slotBar.HasBombSlot)
                onCardPlaced.Subscribe(_ => CheckBombSlot());
        }

        public void Match()
        {
            var slotsBySuitMap = new Dictionary<Suits, List<Slot>>();

            foreach (var slot in _slotBar.Slots)
            {
                if (!slot.HasCard) continue;

                Suits suit = slot.Card.Suit;
                AddSlot(slotsBySuitMap, suit, slot);
            }

            RemoveTripleCards(slotsBySuitMap);
        }

        public Observable<Unit> RemoveCards(List<Card> cards)
        {
            var removedCards = new List<RemovedCard>();
            Observable<Unit> onCompleted = null;

            foreach (var card in cards)
            {
                removedCards.Add(new RemovedCard(card.Rank, card.Coordinates, card.Position));
                onCompleted = card.Destroy();
            }

            foreach (var slot in _slotBar.Slots)
            {
                if (slot.HasCard && slot.Card.IsDestroyed)
                    slot.Release();
            }

            _cardRemovedSubj.OnNext(removedCards);
            return onCompleted;
        }

        private void RemoveTripleCards<TKey>(Dictionary<TKey, List<Slot>> map)
        {
            foreach (var item in map)
            {
                var slots = item.Value;
                if (slots.Count >= 3)
                {
                    var cards = new List<Card>();
                    cards.Capacity = 3;

                    foreach (var slot in slots)
                        cards.Add(slot.Card);

                    RemoveCards(cards);
                }
            }
        }

        private void AddSlot<TKey>(Dictionary<TKey, List<Slot>> map, TKey key, Slot slot)
        {
            if (!map.ContainsKey(key))
                map[key] = new();

            map[key].Add(slot);
        }

        private void CheckBombSlot()
        {
            if (!_slotBar.BombSlot.HasCard) return;

            RemoveCards(new List<Card>() { _slotBar.BombSlot.Card });

            _slotBar.BombSlot.Release();
        }

        public sealed class RemovedCard
        {
            public readonly Ranks Rank;
            public readonly Vector2Int Coordinates;
            public readonly Vector3 Position;

            public RemovedCard(Ranks rank, Vector2Int coordinates, Vector3 position)
            {
                Rank = rank;
                Coordinates = coordinates;
                Position = position;
            }
        }
    }
}
