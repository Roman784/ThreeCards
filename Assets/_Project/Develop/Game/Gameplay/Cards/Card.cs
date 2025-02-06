using GameplayServices;
using UnityEngine;

namespace Gameplay
{
    public class Card
    {
        public CardView View { get; private set; }

        public bool IsMarked { get; private set; }
        public Suits Suit { get; private set; }
        public Ranks Rank { get; private set; }

        private CardMatchingService _cardMatchingService;

        public Card(CardView view)
        {
            View = view;

            View.OnPicked.AddListener(Pick);
        }

        public void Mark(Suits suit, Ranks rank)
        {
            IsMarked = true;
            Suit = suit;
            Rank = rank;

            View.Mark(suit, rank);
        }

        public void SetMatchingService(CardMatchingService service)
        {
            _cardMatchingService = service;
        }

        public void Move(Vector2 position)
        {
            View.Move(position);
        }

        private void Pick()
        {
            _cardMatchingService.PlaceCard(this);
        }
    }
}
