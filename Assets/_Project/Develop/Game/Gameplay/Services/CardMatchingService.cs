using Gameplay;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace GameplayServices
{
    public class CardMatchingService
    {
        private List<Slot> _slots = new();

        private Subject<List<RemovedCard>> _cardRemovedSubj = new();
        public Observable<List<RemovedCard>> OnCardsRemoved => _cardRemovedSubj;

        public CardMatchingService(List<Slot> slots)
        {
            _slots = slots;
        }

        public void Match()
        {
            var slotsBySuitMap = new Dictionary<Suits, List<Slot>>();

            foreach (var slot in _slots)
            {
                if (!slot.HasCard) continue;

                Suits suit = slot.Card.Suit;
                AddSlot(slotsBySuitMap, suit, slot);
            }

            RemoveTripleCards(slotsBySuitMap);
        }

        private void RemoveTripleCards<TKey>(Dictionary<TKey, List<Slot>> map)
        {
            var removedCards = new List<RemovedCard>();

            foreach (var item in map)
            {
                var slots = item.Value;
                if (slots.Count >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var slot = slots[i];
                        var card = slot.Card;

                        removedCards.Add(new RemovedCard(card.Rank, card.GetPosition()));

                        slot.RemoveCard();
                    }
                }
            }

            if (removedCards.Count > 0)
                _cardRemovedSubj.OnNext(removedCards);
        }

        private void AddSlot<TKey>(Dictionary<TKey, List<Slot>> map, TKey key, Slot slot)
        {
            if (!map.ContainsKey(key))
                map[key] = new();

            map[key].Add(slot);
        }

        public sealed class RemovedCard
        {
            public readonly Ranks Rank;
            public readonly Vector3 Position;

            public RemovedCard(Ranks rank, Vector3 position)
            {
                Rank = rank;
                Position = position;
            }
        }
    }
}
