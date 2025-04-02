using CameraUtils;
using Gameplay;
using GameplayServices;
using GameRoot;
using GameState;
using Settings;
using System.Collections;
using UI;
using UnityEngine;
using Zenject;
using R3;
using BonusWhirlpool;
using BonusWhirlpoolService;
using System.Linq;
using System.Collections.Generic;
using GameplayRoot;
using DG.Tweening;
using Audio;

namespace BonusWhirlpoolRoot
{
    public class BonusWhirlpoolEntryPoint : SceneEntryPoint
    {
        private IGameStateProvider _gameStateProvider;
        private UIRootView _uiRoot;
        private BonusWhirlpoolUI _bonusWhirlpoolUI;
        private BonusWhirlpoolPopUpProvider _popUpProvider;
        private ISettingsProvider _settingsProvider;
        private BonusWhirlpoolSlotBar _slotBar;
        private CardFactory _cardFactory;
        private AudioPlayer _audioPlayer;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               UIRootView uiRoot,
                               BonusWhirlpoolUI bonusWhirlpoolUI,
                               BonusWhirlpoolPopUpProvider popUpProvider,
                               ISettingsProvider settingsProvider,
                               BonusWhirlpoolSlotBar slotBar,
                               CardFactory cardFactory,
                               AudioPlayer audioPlayer)
        {
            _gameStateProvider = gameStateProvider;
            _uiRoot = uiRoot;
            _bonusWhirlpoolUI = bonusWhirlpoolUI;
            _popUpProvider = popUpProvider;
            _settingsProvider = settingsProvider;
            _slotBar = slotBar;
            _cardFactory = cardFactory;
            _audioPlayer = audioPlayer;
        }

        public override IEnumerator Run<T>(T enterParams)
        {
            yield return Run(enterParams.As<BonusWhirlpoolEnterParams>());
        }

        private IEnumerator Run(BonusWhirlpoolEnterParams enterParams)
        {
            var isLoaded = false;

            _gameStateProvider.LoadGameState().Subscribe(_ =>
            {
                // Settings.
                var gameSettings = _settingsProvider.GameSettings;
                var cardWhirlpoolSettings = gameSettings.BonusWhirlpoolSettings.CardSettings;
                var audioSettings = gameSettings.AudioSettings;

                // Field setup.
                var slots = _slotBar.CreateSlots();
                var cardPlacingService = new CardPlacingService(_slotBar);
                var onCardPlaced = cardPlacingService.OnCardPlaced;

                var cardMatchingService = new CardMatchingService(_slotBar, onCardPlaced);
                var onCardsRemoved = cardMatchingService.OnCardsRemoved;

                onCardsRemoved.Subscribe(_ => cardPlacingService.ShiftCards());

                var cardMarkingService = new CardMarkingService();
                var cardWhirlpoolService = new CardWhirlpoolService(_cardFactory, cardWhirlpoolSettings, cardPlacingService, cardMarkingService);

                var whirlpoolCards = cardWhirlpoolService.Start();

                // UI.
                _uiRoot.AttachSceneUI(_bonusWhirlpoolUI.gameObject);
                _bonusWhirlpoolUI.BindViews();

                _bonusWhirlpoolUI.InitChips(onCardsRemoved);
                _bonusWhirlpoolUI.SetCurrentLevelNumber(enterParams.CurrentLevelNumber);
                _bonusWhirlpoolUI.SetCards(whirlpoolCards);

                var onTimerOver = _bonusWhirlpoolUI.StartTimer();

                // Audio.
                _audioPlayer.PlayOneShot(audioSettings.CardAudioSettings.RotationSound);
                onTimerOver.Subscribe(_ => _audioPlayer.PlayOneShot(audioSettings.CardAudioSettings.RotationSound));

                // End bonus.
                onTimerOver.Subscribe(_ =>
                {
                    _audioPlayer.PlayOneShot(audioSettings.CardAudioSettings.RotationSound);
                    whirlpoolCards.ForEach(c => c.Card.Close(playSound: false));

                    DOVirtual.DelayedCall(1f, () =>
                    {
                        var gameplayEnterParams = new GameplayEnterParams(enterParams.CurrentLevelNumber, 0f);
                        _popUpProvider.OpenTimeOverPopUp(gameplayEnterParams);
                    });
                });

                isLoaded = true;
            });

            yield return new WaitUntil(() => isLoaded);
        }
    }
}
