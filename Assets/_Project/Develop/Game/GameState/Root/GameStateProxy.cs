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
        public ReactiveProperty<int> LastPassedLevelNumber { get; }
        public ReactiveProperty<float> AudioVolume { get; }

        public GameStateProxy(GameState gameState, IGameStateProvider gameStateProvider)
        {
            _gameState = gameState;
            _gameStateProvider = gameStateProvider;

            Chips = new(_gameState.Chips);
            LastPassedLevelNumber = new(_gameState.LastPassedLevelNumber);
            AudioVolume = new(_gameState.AudioVolume);

            Chips.Skip(1).Subscribe(value => ChangeChips(value));
            LastPassedLevelNumber.Skip(1).Subscribe(value => ChangeLastPassedLevelNumber(value));
            AudioVolume.Skip(1).Subscribe(value => ChangeAudioVolume(value));
        }

        private void ChangeChips(int value)
        {
            _gameState.Chips = value;
            _gameStateProvider.SaveGameState();
        }

        private void ChangeLastPassedLevelNumber(int value)
        {
            _gameState.LastPassedLevelNumber = value;
            _gameStateProvider.SaveGameState();
        }

        private void ChangeAudioVolume(float value)
        {
            _gameState.AudioVolume = value;
            _gameStateProvider.SaveGameState();
        }
    }
}
