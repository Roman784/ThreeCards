using Gameplay;
using System.Collections.Generic;

namespace GameplayServices
{
    public class CardMatchingService
    {
        private List<Slot> _slots = new();

        public void Init(List<Slot> slots)
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
                    break;
                }
            }
        }
    }
}
