using Gameplay;
using GameplayServices;
using GameState;
using R3;
using UnityEngine;
using Zenject;

namespace GameplayRoot
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        private CardLayoutService _cardLayoutService;
        private CardMarkingService _cardMarkingService;

        [Inject]
        private void Construct(CardLayoutService cardLayoutService, CardMarkingService cardMarkingService)
        {
            _cardLayoutService = cardLayoutService;
            _cardMarkingService = cardMarkingService;
        }

        public void Run(GameplayEnterParams enterParams)
        {
            Debug.Log($"Level number {enterParams.LevelNumber}");
            Debug.Log("Gameplay scene loaded");

            Card[,] cards = _cardLayoutService.SetUp(enterParams.LevelNumber);
            _cardMarkingService.Mark(cards);
        }
    }
}
