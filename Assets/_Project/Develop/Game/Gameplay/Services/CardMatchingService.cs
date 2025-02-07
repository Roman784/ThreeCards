using Gameplay;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GameplayServices
{
    public class CardMatchingService
    {
        private List<Slot> _slots = new();

        public UnityEvent<Card> OnCardPlaced = new();

        public CardMatchingService(List<Slot> slots)
        {
            _slots = slots;
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
    }
}
