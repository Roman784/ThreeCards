using Gameplay;
using GameplayServices;
using GameState;
using R3;
using Settings;
using UI;
using UnityEngine;
using Utils;
using Zenject;

namespace GameplayRoot
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private UIRootView _uiRoot;
        private GameplayUI _gameplayUIPrefab;
        private ISettingsProvider _settingsProvider;
        private SlotBar _slotBar;
        private CardFactory _cardFactory;

        [Inject]
        private void Construct(UIRootView uiRoot,
                               GameplayUI gameplayUIPrefab,
                               ISettingsProvider settingsProvider,
                               SlotBar slotBar,
                               CardFactory cardFactory)
        {
            _uiRoot = uiRoot;
            _gameplayUIPrefab = gameplayUIPrefab;
            _settingsProvider = settingsProvider;
            _slotBar = slotBar;
            _cardFactory = cardFactory;
        }

        public void Run(GameplayEnterParams enterParams)
        {
            Debug.Log($"Level number {enterParams.LevelNumber}");
            Debug.Log("Gameplay scene loaded");

            // UI.
            var gameplayUI = Instantiate(_gameplayUIPrefab);
            _uiRoot.AttachSceneUI(gameplayUI.gameObject);

            // Settings data.
            var layouts = _settingsProvider.GameSettings.CardLayoutsSettings;
            var layout = layouts.GetLayout(enterParams.LevelNumber);

            // Services.
            var cardLayoutService = new CardLayoutService(layouts, _cardFactory);
            var cardMarkingService = new CardMarkingService();
            var cardMatchingService = new CardMatchingService();

            // Slots setup.
            var slots = _slotBar.CreateSlots();
            cardMatchingService.Init(slots);

            // Cards setup.
            var cards = cardLayoutService.SetUp(layout);
            cardMarkingService.Mark(cards, layout.CardSpreadRange);

            foreach (var card in cards)
            {
                if (card == null) continue;
                card.SetMatchingService(cardMatchingService);
            }
        }
    }
}
