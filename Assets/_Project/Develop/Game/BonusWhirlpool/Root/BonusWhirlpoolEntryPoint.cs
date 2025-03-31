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
        private ShakyCamera _shakyCamera;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               UIRootView uiRoot,
                               BonusWhirlpoolUI bonusWhirlpoolUI,
                               BonusWhirlpoolPopUpProvider popUpProvider,
                               ISettingsProvider settingsProvider,
                               BonusWhirlpoolSlotBar slotBar,
                               CardFactory cardFactory,
                               ShakyCamera shakyCamera)
        {
            _gameStateProvider = gameStateProvider;
            _uiRoot = uiRoot;
            _bonusWhirlpoolUI = bonusWhirlpoolUI;
            _popUpProvider = popUpProvider;
            _settingsProvider = settingsProvider;
            _slotBar = slotBar;
            _cardFactory = cardFactory;
            _shakyCamera = shakyCamera;
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
                var cardWhirlpoolSettings = _settingsProvider.GameSettings.BonusWhirlpoolSettings.CardSettings;
                
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

                // End bonus.
                onTimerOver.Subscribe(_ =>
                {
                    whirlpoolCards.ForEach(c => c.Card.Close());

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
