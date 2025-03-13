using Gameplay;
using GameplayServices;
using GameState;
using Settings;
using UI;
using UnityEngine;
using Zenject;
using R3;
using Utils;
using CameraUtils;
using System.Collections;
using GameRoot;

namespace GameplayRoot
{
    public class GameplayEntryPoint : SceneEntryPoint
    {
        private IGameStateProvider _gameStateProvider;
        private UIRootView _uiRoot;
        private GameplayUI _gameplayUI;
        private ISettingsProvider _settingsProvider;
        private SlotBar _slotBar;
        private CardFactory _cardFactory;
        private ShakyCamera _shakyCamera;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               UIRootView uiRoot,
                               GameplayUI gameplayUI,
                               ISettingsProvider settingsProvider,
                               SlotBar slotBar,
                               CardFactory cardFactory,
                               ShakyCamera shakyCamera)
        {
            _gameStateProvider = gameStateProvider;
            _uiRoot = uiRoot;
            _gameplayUI = gameplayUI;
            _settingsProvider = settingsProvider;
            _slotBar = slotBar;
            _cardFactory = cardFactory;
            _shakyCamera = shakyCamera;
        }

        public override IEnumerator Run<T>(T enterParams)
        {
            yield return Run(enterParams.As<GameplayEnterParams>());
        }

        private IEnumerator Run(GameplayEnterParams enterParams)
        {
            var isLoaded = false;

            _gameStateProvider.LoadGameState().Subscribe(_ => 
            {
                // Settings data.
                var gameSettings = _settingsProvider.GameSettings;
                var layouts = gameSettings.CardLayoutsSettings;
                var layout = layouts.GetLayout(enterParams.LevelNumber);

                // Fiel setup.
                var slots = _slotBar.CreateSlots();

                var cardPlacingService = new CardPlacingService(slots);
                var onCardPlaced = cardPlacingService.OnCardPlaced;
                var onCardReadyToPlaced = cardPlacingService.OnCardReadyToPlaced;

                var cardMatchingService = new CardMatchingService(slots, onCardPlaced);
                var onCardsRemoved = cardMatchingService.OnCardsRemoved;

                onCardsRemoved.Subscribe(_ => cardPlacingService.ShiftCards());

                var cardLayoutService = new CardLayoutService(layouts, _cardFactory, cardPlacingService);
                var cardMarkingService = new CardMarkingService();

                var cardsMap = cardLayoutService.SetUp(layout);

                var fieldService = new FieldService(cardsMap, _slotBar);

                cardMarkingService.Mark(fieldService, layout.CardSpreadRange);

                // Animations.
                var cardFlippingService = new CardFlippingService(fieldService, onCardReadyToPlaced, onCardsRemoved);
                var fieldAnimationService = new FieldAnimationService(fieldService, cardFlippingService);
                var layOutAnimationCompleted = fieldAnimationService.LayOutCards();

                // UI.
                var fieldShufflingService = new FieldShufflingService(fieldService, cardFlippingService);
                var magicStickService = new MagicStickService(fieldService, cardMatchingService, cardLayoutService);
                var levelRestarterService = new LevelRestarterService(enterParams, fieldService, _shakyCamera);
                var totalCardCount = CollectionsCounter.CountOfNonNullItems(cardsMap);

                _uiRoot.AttachSceneUI(_gameplayUI.gameObject);
                _gameplayUI.BindViews();

                _gameplayUI.SetGameplayEnterParams(enterParams);

                _gameplayUI.SetLevelNumber(enterParams.LevelNumber);
                _gameplayUI.InitProgressBar(totalCardCount, onCardsRemoved);
                _gameplayUI.InitChips(onCardsRemoved);

                _gameplayUI.SetToolsServcies(fieldShufflingService, magicStickService, levelRestarterService);
                layOutAnimationCompleted.Subscribe(_ => _gameplayUI.EnableTools());

                // Winning and losing.
                var gameCompletionService = new GameCompletionService(onCardsRemoved, onCardPlaced,
                                                                      _gameStateProvider, _settingsProvider,
                                                                      enterParams, _gameplayUI, fieldService);

                isLoaded = true;
            });

            yield return new WaitUntil(() => isLoaded);
        }
    }
}
