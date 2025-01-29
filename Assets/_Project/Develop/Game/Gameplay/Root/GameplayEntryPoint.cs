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

        [Inject]
        private void Construct( CardLayoutService cardLayoutService)
        {
            _cardLayoutService = cardLayoutService;
        }

        public void Run(GameplayEnterParams enterParams)
        {
            Debug.Log($"Level number {enterParams.LevelNumber}");
            Debug.Log("Gameplay scene loaded");

            _cardLayoutService.SetUp(enterParams.LevelNumber);
        }
    }
}
