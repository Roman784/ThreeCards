using R3;
using UnityEngine;

namespace Gameplay
{
    public class Slot
    {
        public SlotView View { get; private set; }

        public bool HasCard { get; private set; }
        public Card Card { get; private set; }
        public bool IsDestroyed { get; private set; }

        public Slot(SlotView view)
        {
            View = view;
            IsDestroyed = false;
        }

        public Observable<Unit> PlaceCard(Card card)
        {
            HasCard = true;
            Card = card;

            return card.Place(View.transform);
        }

        public void Release()
        {
            HasCard = false;
        }

        public void SetParent(Transform parent)
        {
            View.SetParent(parent);
        }

        public void SetPosition(Vector3 position)
        {
            View.SetPosition(position);
        }

        public void Destroy()
        {
            IsDestroyed = true;
            View.Destroy();
        }
    }
}
