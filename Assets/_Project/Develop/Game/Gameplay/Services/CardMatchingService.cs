using Gameplay;
using System.Collections.Generic;
using R3;
using Currencies;

namespace GameplayServices
{
    public class CardMatchingService
    {
        private List<Slot> _slots = new();
        private ChipsCounter _chipsCounter;

        private Subject<Unit> _cardRemovedSubj = new();
        public Observable<Unit> OnCardsRemoved => _cardRemovedSubj;

        public CardMatchingService(List<Slot> slots, ChipsCounter chipsCounter)
        {
            _slots = slots;
            _chipsCounter = chipsCounter;
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
            bool wasRemoved = false;

            foreach (var item in map)
            {
                var slots = item.Value;
                if (slots.Count >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var slot = slots[i];
                        var card = slot.Card;

                        var chipsCount = CardMarkingMapper.GetRankValue(card.Rank);
                        var cardPosition = card.GetPosition();
                        _chipsCounter.Add(chipsCount, cardPosition);

                        slot.RemoveCard();
                    }
                    wasRemoved = true;
                }
            }

            if (wasRemoved)
                _cardRemovedSubj.OnNext(Unit.Default);
        }

        private void AddSlot<TKey>(Dictionary<TKey, List<Slot>> map, TKey key, Slot slot)
        {
            if (!map.ContainsKey(key))
                map[key] = new();

            map[key].Add(slot);
        }
    }
}
