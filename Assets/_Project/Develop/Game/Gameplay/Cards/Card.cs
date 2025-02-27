using DG.Tweening;
using GameplayServices;
using R3;
using UnityEngine;

namespace Gameplay
{
    public class Card
    {
        private CardView _view;

        private CardPlacingService _cardPlacingService;

        public Vector2Int Coordinates { get; private set; }
        public bool IsClosed { get; private set; }
        public bool IsMarked { get; private set; }
        public bool IsPlaced { get; private set; }
        public Suits Suit { get; private set; }
        public Ranks Rank { get; private set; }
        public bool IsDestroyed { get; private set; }
        public Vector3 Position => _view.GetPosition();

        public Card(CardView view)
        {
            IsClosed = true;
            IsMarked = false;

            _view = view;

            _view.OnPicked.Subscribe(_ => Pick());
        }

        public void SetCoordinates(Vector2Int coordinates) => Coordinates = coordinates;
        public void SetPosition(Vector3 position) => _view.SetPosition(position);

        public void Mark(Suits suit, Ranks rank)
        {
            IsMarked = true;
            Suit = suit;
            Rank = rank;

            _view.Mark(suit, rank);
            _view.Close(true);
        }

        public void SetPlacingService(CardPlacingService service)
        {
            _cardPlacingService = service;
        }

        public Observable<Unit> Place(Transform slot)
        {
            IsPlaced = true;
            return _view.Place(slot);
        }

        public Observable<Unit> Move(Vector3 position, Ease ease = Ease.OutQuad, float moveDuration = 0, float speedMultiplyer = 1)
        {
            return _view.Move(position, ease, moveDuration, speedMultiplyer);
        }

        public Observable<Unit> Close(bool instantly = false)
        {
            IsClosed = true;
            return _view.Close(instantly);
        }

        public Observable<Unit> Open()
        {
            IsClosed = false;
            return _view.Open();
        }

        public void PutDown()
        {
            _view.PutDown();
        }

        public void Disable()
        {
            _view.Disable();
        }

        public Observable<Unit> Explode()
        {
            IsDestroyed = true;
            return _view.Explode();
        }

        public Observable<Unit> Destroy()
        {
            IsDestroyed = true;
            var onDestroyed = _view.Destroy();

            onDestroyed.Subscribe(_ =>
            {
                Object.Destroy(_view.gameObject);
            });

            return onDestroyed;
        }

        private void Pick()
        {
            if (IsClosed || IsPlaced || IsDestroyed) return;
            _cardPlacingService.PlaceCard(this);
        }
    }
}
