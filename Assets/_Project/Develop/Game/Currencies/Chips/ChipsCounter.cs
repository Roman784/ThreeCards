using GameState;
using Zenject;
using R3;
using UnityEngine;
using GameplayServices;
using Gameplay;
using System.Collections.Generic;
using UI;
using Audio;
using Settings;
using static GameplayServices.CardMatchingService;

namespace Currencies
{
    public class ChipsCounter
    {
        private int _chipsCount;
        private ChipsCounterView _view;

        private IGameStateProvider _gameStateProvider;
        private PopUpProvider _popUpProvider;
        private AudioPlayer _audioPlayer;
        private Settings.AudioSettings _audioSettings;

        public int Count => _chipsCount;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider, PopUpProvider popUpProvider,
                               AudioPlayer audioPlayer, ISettingsProvider settingsProvider)
        {
            _gameStateProvider = gameStateProvider;
            _popUpProvider = popUpProvider;
            _audioPlayer = audioPlayer;
            _audioSettings = settingsProvider.GameSettings.AudioSettings;
        }

        public void BindView(ChipsCounterView view)
        {
            _view = view;

            _view.OnGetAdvertisingChips += () =>
            {
                _audioPlayer.PlayOneShot(_audioSettings.UIAudioSettings.ButtonClickSound);
                _popUpProvider.OpenAdvertisingChipsPopUp();
            };
        }

        public void InitChips(Observable<List<CardMatchingService.RemovedCard>> onCardsRemoved = null)
        {
            _chipsCount = _gameStateProvider.GameState.Chips.Value;
            _view?.SetCurrentCount(_chipsCount);

            Observable<Unit> onCollected = null;
            onCardsRemoved?.Subscribe(removedCards =>
            {
                foreach (var card in removedCards)
                {
                    var chipsCount = CardMarkingMapper.GetRankValue(card.Rank) / 2;
                    onCollected = Add(chipsCount, card.Position);
                }

                var clip = _audioSettings.ChipsAudioSettings.CollectionSound;
                onCollected?.Subscribe(_ => _audioPlayer.PlayAnyTimes(clip, removedCards.Count, 0.1f));
            });
        }

        public void Reduce(int value)
        {
            if (value > _chipsCount)
                value = _chipsCount;

            _chipsCount -= value;
            _gameStateProvider.GameState.Chips.Value = _chipsCount;

            var clip = _audioSettings.ChipsAudioSettings.ReduceSound;
            _audioPlayer.PlayOneShot(clip);

            _view?.ChangeCounter(_chipsCount);
        }

        public void Add(int value, bool changeView = true, bool instantly = true, bool playSound = false)
        {
            _chipsCount += value;
            _gameStateProvider.GameState.Chips.Value = _chipsCount;

            if (playSound)
            {
                var clip = _audioSettings.ChipsAudioSettings.CollectionSound;
                _audioPlayer.PlayAnyTimes(clip, 10, 0.075f);
            }

            if (changeView)
                if (instantly)
                    _view?.SetCurrentCount(_chipsCount);
                else
                    _view?.ChangeCounter(_chipsCount);
        }

        private Observable<Unit> Add(int value, Vector3 initialColectionPosition)
        {
            var onCompleted = new Subject<Unit>();

            Add(value, false);
            _view.AnimateCollection(value, initialColectionPosition).Subscribe(_ => 
            {
                _view?.ChangeCounter(_chipsCount);
                onCompleted.OnNext(Unit.Default);
                onCompleted.OnCompleted();
            });

            return onCompleted;
        }
    }
}
