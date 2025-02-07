using UnityEngine;

namespace Gameplay
{
    public class Slot
    {
        public SlotView View { get; private set; }

        public bool HasCard { get; private set; }
        public Card Card { get; private set; }

        public Slot(SlotView view)
        {
            View = view;
        }

        public void PlaceCard(Card card)
        {
            HasCard = true;
            Card = card;

            card.Move(View.transform);
        }

        public void RemoveCard()
        {
            HasCard = false;
            Card.Destroy();
        }

        public void SetParent(Transform parent)
        {
            View.SetParent(parent);
        }

        public void SetPosition(Vector2 position)
        {
            View.SetPosition(position);
        }
    }
}
