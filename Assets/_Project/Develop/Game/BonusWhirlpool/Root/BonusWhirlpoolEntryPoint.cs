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

namespace BonusWhirlpoolRoot
{
    public class BonusWhirlpoolEntryPoint : SceneEntryPoint
    {
        private IGameStateProvider _gameStateProvider;
        private UIRootView _uiRoot;
        private BonusWhirlpoolUI _bonusWhirlpoolUI;
        private GameplayPopUpProvider _popUpProvider;
        private ISettingsProvider _settingsProvider;
        private BonusWhirlpoolSlotBar _slotBar;
        private CardFactory _cardFactory;
        private ShakyCamera _shakyCamera;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               UIRootView uiRoot,
                               BonusWhirlpoolUI bonusWhirlpoolUI,
                               ISettingsProvider settingsProvider,
                               BonusWhirlpoolSlotBar slotBar,
                               CardFactory cardFactory,
                               ShakyCamera shakyCamera)
        {
            _gameStateProvider = gameStateProvider;
            _uiRoot = uiRoot;
            _bonusWhirlpoolUI = bonusWhirlpoolUI;
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
                var cardWhirlpoolSettings = _settingsProvider.GameSettings.CardWhirlpoolSettings;
                
                // Field setup.
                var slots = _slotBar.CreateSlots();
                var cardPlacingService = new CardPlacingService(slots);

                var cardWhirlpoolService = new CardWhirlpoolService(_cardFactory, cardWhirlpoolSettings, cardPlacingService);
                var cardMarkingService = new CardMarkingService();

                var whirlpoolCards = cardWhirlpoolService.Start();
                var cards = whirlpoolCards.Select(c => c.Card).ToList();

                cardMarkingService.MarkRandom(cards);
                cards.ForEach(c => c.Open(true));

                //UI.
                _uiRoot.AttachSceneUI(_bonusWhirlpoolUI.gameObject);
                _bonusWhirlpoolUI.BindViews();

                _bonusWhirlpoolUI.InitChips();
                _bonusWhirlpoolUI.SetCurrentLevelNumber(enterParams.CurrentLevelNumber);

                isLoaded = true;
            });

            yield return new WaitUntil(() => isLoaded);
        }
    }
}
