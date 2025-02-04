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

        [Inject]
        private void Construct(UIRootView uiRoot,
                               GameplayUI gameplayUIPrefab,
                               ISettingsProvider settingsProvider,
                               CardLayoutService cardLayoutService,
                               CardMarkingService cardMarkingService,
                               CardMatchingService cardMatchingService)
        {
            _uiRoot = uiRoot;
            _gameplayUIPrefab = gameplayUIPrefab;
            _settingsProvider = settingsProvider;
            _cardLayoutService = cardLayoutService;
            _cardMarkingService = cardMarkingService;
            _cardMatchingService = cardMatchingService;
        }

        public void Run(GameplayEnterParams enterParams)
        {
            Debug.Log($"Level number {enterParams.LevelNumber}");
            Debug.Log("Gameplay scene loaded");

            GameplayUI gameplayUI = Instantiate(_gameplayUIPrefab);
            _uiRoot.AttachSceneUI(gameplayUI.gameObject);

            CardLayoutsSettings layouts = _settingsProvider.GameSettings.CardLayoutsSettings;
            CardLayoutSettings layout = layouts.GetLayout(enterParams.LevelNumber);

            Card[,] cards = _cardLayoutService.SetUp(layout);
            Coroutines.StartRoutine(_cardMarkingService.Mark(cards, layout.CardSpreadRange));

            _cardMatchingService.CreateSlots(gameplayUI);
        }
    }
}
