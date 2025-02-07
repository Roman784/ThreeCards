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

        private bool _isClosed;

        private CardMatchingService _cardMatchingService;

        public Card(CardView view)
        {
            _isClosed = true;

            View = view;
            View.OnPicked.AddListener(Pick);
        }

        public void Mark(Suits suit, Ranks rank)
        {
            IsMarked = true;
            Suit = suit;
            Rank = rank;

            View.Mark(suit, rank);
            View.Close(true);
        }

        public void SetMatchingService(CardMatchingService service)
        {
            _cardMatchingService = service;
        }

        public void Place(Transform slot)
        {
            View.Place(slot);
        }

        public void Close(bool instantly = false)
        {
            _isClosed = true;
            View.Close(instantly);
        }

        public void Open()
        {
            _isClosed = false;
            View.Open();
        }

        public void Destroy()
        {
            Object.Destroy(View.gameObject);
        }

        private void Pick()
        {
            if (_isClosed) return;

            _cardMatchingService.PlaceCard(this);
        }
    }
}
