using UnityEngine;

namespace Gameplay
{
    public class Slot : MonoBehaviour
    {
        public bool HasCard { get; private set; }
        public Card Card { get; private set; }

        private void Awake()
        {
            HasCard = false;
            Card = null;
        }

        public void PlaceCard(Card card)
        {
            HasCard = true;
            Card = card;

            // card.transform.localScale = transform.localScale;
            card.Move(transform.position);
        }

        public void RemoveCard()
        {
            HasCard = false;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }
    }
}
