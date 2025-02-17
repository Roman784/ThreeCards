using R3;
using UnityEngine;
using Zenject;

namespace GameState
{
    public class GameStateProxy
    {
        private readonly GameState _gameState;

        public GameState State => _gameState;
        public ReactiveProperty<int> Chips { get; }


        public GameStateProxy(GameState gameState, IGameStateProvider stateProvider)
        {
            _gameState = gameState;

            Chips = new(_gameState.Chips);
            Chips.Skip(1).Subscribe(value => 
            {
                _gameState.Chips = value;
                stateProvider.SaveGameState();
            });
        }
    }
}
