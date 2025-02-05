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
        private CardLayoutService _cardLayoutService;
        private CardMarkingService _cardMarkingService;
        private CardMatchingService _cardMatchingService;
        private SlotBar _slotBar;

        [Inject]
        private void Construct(UIRootView uiRoot,
                               GameplayUI gameplayUIPrefab,
                               ISettingsProvider settingsProvider,
                               CardLayoutService cardLayoutService,
                               CardMarkingService cardMarkingService,
                               CardMatchingService cardMatchingService,
                               SlotBar slotBar)
        {
            _uiRoot = uiRoot;
            _gameplayUIPrefab = gameplayUIPrefab;
            _settingsProvider = settingsProvider;
            _cardLayoutService = cardLayoutService;
            _cardMarkingService = cardMarkingService;
            _cardMatchingService = cardMatchingService;
            _slotBar = slotBar;
        }

        public void Run(GameplayEnterParams enterParams)
        {
            Debug.Log($"Level number {enterParams.LevelNumber}");
            Debug.Log("Gameplay scene loaded");

            var gameplayUI = Instantiate(_gameplayUIPrefab);
            _uiRoot.AttachSceneUI(gameplayUI.gameObject);

            var layouts = _settingsProvider.GameSettings.CardLayoutsSettings;
            var layout = layouts.GetLayout(enterParams.LevelNumber);

            var cards = _cardLayoutService.SetUp(layout);
            _cardMarkingService.Mark(cards, layout.CardSpreadRange);

            var slots = _slotBar.CreateSlots();
            _cardMatchingService.Init(slots);
        }
    }
}
