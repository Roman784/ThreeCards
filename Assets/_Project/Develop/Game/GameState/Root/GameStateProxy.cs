using R3;
using UnityEngine;
using Zenject;

namespace GameState
{
    public class GameStateProxy
    {
        private readonly GameState _gameState;

        public ReactiveProperty<float> SoundVolume { get; }

        private IGameStateProvider _stateProvider;

        [Inject]
        private void Construct(IGameStateProvider stateProvider)
        {
            _stateProvider = stateProvider;
        }

        public GameStateProxy(GameState gameState)
        {
            _gameState = gameState;

            SoundVolume.Subscribe(value =>
            {
                _gameState.SoundVolume = value;
                _stateProvider.SaveGameState();
                Debug.Log($"Sound volume changed {value}");
            });
        }
    }
}
