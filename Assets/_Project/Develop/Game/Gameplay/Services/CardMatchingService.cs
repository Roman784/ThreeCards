using Gameplay;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GameplayServices
{
    public class CardMatchingService
    {
        private List<Slot> _slots = new();
        private Dictionary<Suits, List<Slot>> _slotsBySuitMap = new();

        public UnityEvent<Card> OnCardPlaced = new();

        public CardMatchingService(List<Slot> slots)
        {
            _slots = slots;

            OnCardPlaced.AddListener((Card _) => Match());
        }

        public void PlaceCard(Card card)
        {
            foreach (Slot slot in _slots)
            {
                if (!slot.HasCard)
                {
                    slot.PlaceCard(card);
                    OnCardPlaced.Invoke(card);
                    break;
                }
            }
        }

        private void Match()
        {
            _slotsBySuitMap.Clear();

            foreach (Slot slot in _slots)
            {
                if (!slot.HasCard) continue;

                Suits suit = slot.Card.Suit;
                AddSlot(_slotsBySuitMap, suit, slot);
            }

            RemoveTripleCards(_slotsBySuitMap);
        }

        private void RemoveTripleCards<TKey>(Dictionary<TKey, List<Slot>> map)
        {
            foreach (var item in map)
            {
                List<Slot> slots = item.Value;
                if (slots.Count >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        slots[i].RemoveCard();
                    }
                }
            }
        }

        private void AddSlot<TKey>(Dictionary<TKey, List<Slot>> map, TKey key, Slot slot)
        {
            if (!map.ContainsKey(key))
                map[key] = new();

            map[key].Add(slot);
        }
    }
}
