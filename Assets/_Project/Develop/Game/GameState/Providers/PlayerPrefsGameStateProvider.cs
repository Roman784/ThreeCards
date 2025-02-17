using R3;
using UnityEngine;

namespace GameState
{
    public class PlayerPrefsGameStateProvider : IGameStateProvider
    {
        private const string GAME_STATE_KEY = nameof(GAME_STATE_KEY);

        public GameStateProxy GameState { get; private set; }

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
                var gameState = JsonUtility.FromJson<GameState>(json);
                GameState = new GameStateProxy(gameState, this);
            }

            return Observable.Return(GameState);
        }

        public Observable<bool> SaveGameState()
        {
            var json = JsonUtility.ToJson(GameState.State, true);
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
            var gameState = new GameState
            {
                Chips = 10,
            };

            return new GameStateProxy(gameState, this);
        }
    }
}
