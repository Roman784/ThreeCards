using Audio;
using DG.Tweening;
using GameplayServices;
using R3;
using Settings;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class Card
    {
        private CardView _view;

        private CardPlacingService _cardPlacingService;
        private GameplayTools _gameplayTools;
        private AudioPlayer _audioPlayer;
        private CardAudioSettings _audioSettings;

        private Subject<Card> _cardPlacedSubj = new();
        public Observable<Card> OnCardPlaced => _cardPlacedSubj;

        public Vector2Int Coordinates { get; private set; }
        public bool IsClosed { get; private set; }
        public bool IsMarked { get; private set; }
        public bool IsPlaced { get; private set; }
        public Suits Suit { get; private set; }
        public Ranks Rank { get; private set; }
        public bool IsBomb { get; private set; }
        public bool IsDestroyed { get; private set; }
        public Vector3 Position => _view.GetPosition();

        public bool CanDetonate { get; set; }

        public Card(CardView view, bool isBomb, 
                    CardPlacingService placingService, SlotBar slotBar, 
                    AudioPlayer audioPlayer, CardAudioSettings audioSettings)
        {
            IsClosed = true;
            IsMarked = false;
            IsBomb = isBomb;
            CanDetonate = IsBomb;
            _cardPlacingService = placingService;
            _audioPlayer = audioPlayer;
            _audioSettings = audioSettings;

            _view = view;
            _view.OnPicked.Subscribe(_ => Pick());

            if (IsBomb)
            {
                var dragging = _view.EnableDragging();
                dragging.Init(this, slotBar, placingService);
            }
        }

        public void SetCoordinates(Vector2Int coordinates) => Coordinates = coordinates;
        public void SetPosition(Vector3 position)
        {
            if (!IsDestroyed)
                _view.SetPosition(position);
        }

        public void Rotate(Vector3 eulers)
        {
            if (!IsDestroyed && !IsPlaced)
                _view.Rotate(eulers);
        }

        public void Mark(Suits suit, Ranks rank)
        {
            IsMarked = true;
            Suit = suit;
            Rank = rank;

            _view.Mark(suit, rank);
            _view.Close(true);
        }

        public void MarkAsbomb()
        {
            Suits suit = CardMarkingMapper.GetRandomSuits();
            Ranks rank = CardMarkingMapper.GetRandomRank();

            Mark(suit, rank);
            _view.MarkAsBomb();
        }

        public void SetGameplayTools(GameplayTools gameplayTools)
        {
            _gameplayTools = gameplayTools;
        }

        public Observable<Unit> Place(Transform slot)
        {
            IsPlaced = true;
            _cardPlacedSubj.OnNext(this);
            _view.Place(slot);
            return Move(slot.position);
        }

        public Observable<Unit> Move(Vector3 position, Ease ease = Ease.OutQuad, 
                                     float moveDuration = 0, float speedMultiplyer = 1, bool playSound = true)
        {
            //if (playSound)
                //_audioPlayer.PlayOneShot(_audioSettings.MovementSound);

            return _view.Move(position, ease, moveDuration, speedMultiplyer);
        }

        public Observable<Unit> Close(bool instantly = false, bool playSound = true)
        {
            IsClosed = true;

            if (playSound)
                _audioPlayer.PlayOneShot(_audioSettings.RotationSound);
            
            return _view.Close(instantly);
        }

        public Observable<Unit> Open(bool instantly = false, bool playSound = true)
        {
            IsClosed = false;

            if (playSound)
                _audioPlayer.PlayOneShot(_audioSettings.RotationSound);

            return _view.Open(instantly);
        }

        public void PutDown(bool playSound = true)
        {
            if (playSound)
                _audioPlayer.PlayOneShot(_audioSettings.PutDownSound);

            _view.PutDown();
        }

        public void Disable(bool setActive = true)
        {
            _view.Disable(setActive);
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

        public void Pick()
        {
            if (IsBomb)
            {
                _view.Disable(false);
                Hiss().Subscribe(_ =>
                {
                    if (CanDetonate)
                        _gameplayTools.RestartLevel();
                });
                return;
            }

            if (IsClosed || IsPlaced || IsDestroyed) return;
            _cardPlacingService.PlaceCard(this);
        }

        private Observable<Unit> Hiss()
        {
            _audioPlayer.PlayOneShot(_audioSettings.HissSound);
            return _view.Hiss();
        }
    }
}
