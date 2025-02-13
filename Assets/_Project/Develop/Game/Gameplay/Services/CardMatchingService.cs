using Gameplay;
using System.Collections.Generic;
using UnityEngine.Events;
using R3;
using Utils;
using UnityEditorInternal.VR;
using UnityEngine;

namespace GameplayServices
{
    public class CardMatchingService
    {
        private List<Slot> _slots = new();

        public UnityEvent<Card> OnCardPlaced = new();
        public UnityEvent OnCardsRemoved = new();

        private bool _canPlaceCard;

        public CardMatchingService(List<Slot> slots)
        {
            _slots = slots;
            _canPlaceCard = true;

            OnCardsRemoved.AddListener(ShiftCards);
        }

        public void PlaceCard(Card card)
        {
            if (!_canPlaceCard) return;

            foreach (var slot in _slots)
            {
                if (!slot.HasCard)
                {
                    slot.PlaceCard(card).Subscribe(_ => 
                    {
                        Match();
                    });
                    OnCardPlaced.Invoke(card);
                    break;
                }
            }
        }

        private void Match()
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
                List<Slot> slots = item.Value;
                if (slots.Count >= 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        slots[i].RemoveCard();
                    }
                    wasRemoved = true;
                }
            }

            if (wasRemoved)
                OnCardsRemoved.Invoke();
        }

        private void AddSlot<TKey>(Dictionary<TKey, List<Slot>> map, TKey key, Slot slot)
        {
            if (!map.ContainsKey(key))
                map[key] = new();

            map[key].Add(slot);
        }

        private void ShiftCards()
        {
            _canPlaceCard = false;

            Coroutines.Invoke(() =>
            {
                foreach (var slot in _slots)
                {
                    if (!slot.HasCard) continue;

                    Card card = slot.Card;
                    slot.Release();

                    foreach (var newSlot in _slots)
                    {
                        if (newSlot.HasCard) continue;

                        newSlot.PlaceCard(card);
                        break;
                    }
                }

                _canPlaceCard = true;
            }, 0.5f);
        }
    }
}
