using Gameplay;
using GameplayServices;
using GameState;
using Settings;
using UI;
using UnityEngine;
using Zenject;
using R3;

namespace GameplayRoot
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private IGameStateProvider _gameStateProvider;
        private UIRootView _uiRoot;
        private GameplayUI _gameplayUIPrefab;
        private ISettingsProvider _settingsProvider;
        private SlotBar _slotBar;
        private CardFactory _cardFactory;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               UIRootView uiRoot,
                               GameplayUI gameplayUIPrefab,
                               ISettingsProvider settingsProvider,
                               SlotBar slotBar,
                               CardFactory cardFactory)
        {
            _gameStateProvider = gameStateProvider;
            _uiRoot = uiRoot;
            _gameplayUIPrefab = gameplayUIPrefab;
            _settingsProvider = settingsProvider;
            _slotBar = slotBar;
            _cardFactory = cardFactory;
        }

        public void Run(GameplayEnterParams enterParams)
        {
            _gameStateProvider.LoadGameState().Subscribe(_ => 
            {
                // UI.
                var gameplayUI = Instantiate(_gameplayUIPrefab);
                _uiRoot.AttachSceneUI(gameplayUI.gameObject);
                gameplayUI.Init();

                // Settings data.
                var layouts = _settingsProvider.GameSettings.CardLayoutsSettings;
                var layout = layouts.GetLayout(enterParams.LevelNumber);

                // Slots setup.
                var slots = _slotBar.CreateSlots();
                var cardMatchingService = new CardMatchingService(slots);

                // Cards setup.
                var cardLayoutService = new CardLayoutService(layouts, _cardFactory);
                var cardMarkingService = new CardMarkingService();

                var cardsMap = cardLayoutService.SetUp(layout);
                cardMarkingService.Mark(cardsMap, layout.CardSpreadRange);

                foreach (var card in cardsMap)
                {
                    if (card == null) continue;
                    card.SetMatchingService(cardMatchingService);
                }

                // Animations.
                var cardFlippingService = new CardFlippingService(cardsMap, cardMatchingService);
                var fieldAnimationService = new FieldAnimationService(cardsMap, cardFlippingService);
                fieldAnimationService.LayOutCards();
            });
        }
    }
}
