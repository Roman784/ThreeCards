using R3;
using UnityEngine;

namespace GameState
{
    public class PlayerPrefsGameStateProvider : IGameStateProvider
    {
        private const string GAME_STATE_KEY = nameof(GAME_STATE_KEY);

        public GameStateProxy GameState {  get; private set; }

        private GameState _gameStateOrigin;

        public Observable<GameStateProxy> LoadGameState()
        {
            if (!PlayerPrefs.HasKey(GAME_STATE_KEY))
            {
                GameState = CreateGameStateFromSettings();
                SaveGameState();
            }
            else
            {
                var json = PlayerPrefs.GetString(GAME_STATE_KEY);
                _gameStateOrigin = JsonUtility.FromJson<GameState>(json);
                GameState = new GameStateProxy(_gameStateOrigin);
            }

            return Observable.Return(GameState);
        }

        public Observable<bool> SaveGameState()
        {
            var json = JsonUtility.ToJson(_gameStateOrigin, true);
            PlayerPrefs.SetString(GAME_STATE_KEY, json);

            return Observable.Return(true);
        }

        public Observable<bool> ResetGameState()
        {
            GameState = CreateGameStateFromSettings();
            SaveGameState();

            return Observable.Return(true);
        }

        // Балванка.
        private GameStateProxy CreateGameStateFromSettings()
        {
            _gameStateOrigin = new GameState
            {
            };

            return new GameStateProxy(_gameStateOrigin);
        }
    }
}
