using R3;
using UnityEngine;
using Zenject;

namespace GameState
{
    public class GameStateProxy
    {
        private readonly GameState _gameState;
        private readonly IGameStateProvider _gameStateProvider;

        public GameState State => _gameState;

        public ReactiveProperty<int> Chips { get; }
        public ReactiveProperty<GameSessionState> CurrentGameSession { get; }

        public GameStateProxy(GameState gameState, IGameStateProvider gameStateProvider)
        {
            _gameState = gameState;
            _gameStateProvider = gameStateProvider;

            Chips = new(_gameState.Chips);
            CurrentGameSession = new(_gameState.CurrentGameSession);

            Chips.Skip(1).Subscribe(value => ChangeChips(value));
            CurrentGameSession.Skip(1).Subscribe(state => ChangeCurrentGameSession(state));
        }

        private void ChangeChips(int value)
        {
            _gameState.Chips = value;
            _gameStateProvider.SaveGameState();
        }

        private void ChangeCurrentGameSession(GameSessionState state)
        {
            _gameState.CurrentGameSession = state;
            _gameStateProvider.SaveGameState();
        }
    }
}
