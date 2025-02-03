using Gameplay;
using GameplayServices;
using GameState;
using R3;
using Settings;
using UnityEngine;
using Utils;
using Zenject;

namespace GameplayRoot
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private ISettingsProvider _settingsProvider;
        private CardLayoutService _cardLayoutService;
        private CardMarkingService _cardMarkingService;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider,
                               CardLayoutService cardLayoutService,
                               CardMarkingService cardMarkingService)
        {
            _settingsProvider = settingsProvider;
            _cardLayoutService = cardLayoutService;
            _cardMarkingService = cardMarkingService;
        }

        public void Run(GameplayEnterParams enterParams)
        {
            Debug.Log($"Level number {enterParams.LevelNumber}");
            Debug.Log("Gameplay scene loaded");

            CardLayoutsSettings layouts = _settingsProvider.GameSettings.CardLayoutsSettings;
            CardLayoutSettings layout = layouts.GetLayout(enterParams.LevelNumber);

            Card[,] cards = _cardLayoutService.SetUp(layout);
            Coroutines.StartRoutine(_cardMarkingService.Mark(cards, layout.CardSpreadRange));
        }
    }
}
