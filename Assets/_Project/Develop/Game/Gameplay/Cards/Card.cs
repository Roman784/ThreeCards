using GameplayServices;
using R3;
using UnityEngine;

namespace Gameplay
{
    public class Card
    {
        private CardView _view;
        private bool _isClosed;

        private CardMatchingService _cardMatchingService;

        public bool IsMarked { get; private set; }
        public Suits Suit { get; private set; }
        public Ranks Rank { get; private set; }

        public Card(CardView view)
        {
            _isClosed = true;
            IsMarked = false;

            _view = view;
            _view.OnPicked.Subscribe(_ => Pick());
        }

        public Vector3 GetPosition() => _view.GetPosition();
        public void SetPosition(Vector3 position) => _view.SetPosition(position);

        public void Mark(Suits suit, Ranks rank)
        {
            IsMarked = true;
            Suit = suit;
            Rank = rank;

            _view.Mark(suit, rank);
            _view.Close(true);
        }

        public void SetMatchingService(CardMatchingService service)
        {
            _cardMatchingService = service;
        }

        public Observable<Unit> Place(Transform slot)
        {
            return _view.Place(slot);
        }

        public void Close(bool instantly = false)
        {
            _isClosed = true;
            _view.Close(instantly);
        }

        public void Open()
        {
            _isClosed = false;
            _view.Open();
        }

        public void PutDown()
        {
            _view.PutDown();
        }

        public void Disable()
        {
            _view.Disable();
        }

        public void Destroy()
        {
            _view.Destroy().Subscribe(_ => 
            {
                Object.Destroy(_view.gameObject);
            });
        }

        private void Pick()
        {
            if (_isClosed) return;
            _cardMatchingService.PlaceCard(this);
        }
    }
}
