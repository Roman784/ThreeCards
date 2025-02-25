using Gameplay;
using GameplayServices;
using GameState;
using Settings;
using UI;
using UnityEngine;
using Zenject;
using R3;
using Utils;

namespace GameplayRoot
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private IGameStateProvider _gameStateProvider;
        private UIRootView _uiRoot;
        private GameplayUI _gameplayUI;
        private ISettingsProvider _settingsProvider;
        private SlotBar _slotBar;
        private CardFactory _cardFactory;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               UIRootView uiRoot,
                               GameplayUI gameplayUI,
                               ISettingsProvider settingsProvider,
                               SlotBar slotBar,
                               CardFactory cardFactory)
        {
            _gameStateProvider = gameStateProvider;
            _uiRoot = uiRoot;
            _gameplayUI = gameplayUI;
            _settingsProvider = settingsProvider;
            _slotBar = slotBar;
            _cardFactory = cardFactory;
        }

        public void Run(GameplayEnterParams enterParams)
        {
            _gameStateProvider.LoadGameState().Subscribe(_ => 
            {
                // Settings data.
                var layouts = _settingsProvider.GameSettings.CardLayoutsSettings;
                var layout = layouts.GetLayout(enterParams.LevelNumber);

                // Slots setup.
                var slots = _slotBar.CreateSlots();
                var cardMatchingService = new CardMatchingService(slots);
                var cardPlacingService = new CardPlacingService(slots, cardMatchingService);

                // Cards setup.
                var cardLayoutService = new CardLayoutService(layouts, _cardFactory, cardPlacingService);
                var cardMarkingService = new CardMarkingService();

                var cardsMap = cardLayoutService.SetUp(layout);
                var totalCardCount = CollectionsCounter.CountOfNonNullItems(cardsMap);
                cardMarkingService.Mark(cardsMap, layout.CardSpreadRange);

                // Animations.
                var cardFlippingService = new CardFlippingService(cardsMap, cardPlacingService);
                var fieldAnimationService = new FieldAnimationService(cardsMap, cardFlippingService);
                fieldAnimationService.LayOutCards();

                // Game session state.
                var gameSessionStateProvider = new GameSessionStateProvider();

                // UI.
                _uiRoot.AttachSceneUI(_gameplayUI.gameObject);
                _gameplayUI.BindViews();

                _gameplayUI.SetLevelNumber(enterParams.LevelNumber);
                _gameplayUI.InitProgressBar(totalCardCount, cardMatchingService);

                _gameplayUI.InitChips(cardMatchingService);
            });
        }
    }
}
