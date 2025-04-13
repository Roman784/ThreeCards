using R3;
using Settings;
using UnityEngine;
using Zenject;

namespace GameState
{
    public class SDKGameStateProvider : IGameStateProvider
    {
        private SDK.SDK _sdk;
        private InitialGameStateSettings _initialGameStateSettings;

        public GameStateProxy GameState { get; private set; }
        public GameSessionStateProvider GameSessionStateProvider { get; private set; }

        [Inject]
        private void Construct(SDK.SDK sdk, ISettingsProvider settingsProvider)
        {
            _sdk = sdk;
            _initialGameStateSettings = settingsProvider.GameSettings.InitialGameStateSettings;
        }

        public Observable<GameStateProxy> LoadGameState()
        {
            var gameStateLoadedSubj = new Subject<GameStateProxy>();

            _sdk.LoadData().Subscribe(res =>
            {
                if (res != "none")
                {
                    if (res == "" || res == "{}")
                    {
                        GameState = CreateGameStateFromSettings();
                        SaveGameState();
                    }
                    else
                    {
                        var gameState = JsonUtility.FromJson<GameState>(res);
                        GameState = new GameStateProxy(gameState, this);
                    }

                    gameStateLoadedSubj.OnNext(GameState);
                }

                gameStateLoadedSubj.OnNext(null);
            });

            return gameStateLoadedSubj;
        }

        public Observable<bool> ResetGameState()
        {
            GameState = CreateGameStateFromSettings();
            SaveGameState();

            return Observable.Return(true);
        }

        public Observable<bool> SaveGameState()
        {
            var json = JsonUtility.ToJson(GameState.State, true);
            _sdk.SaveData(json);

            return Observable.Return(true);
        }

        private GameStateProxy CreateGameStateFromSettings()
        {
            var gameState = new GameState
            {
                Chips = _initialGameStateSettings.GameState.Chips,
                LastPassedLevelNumber = _initialGameStateSettings.GameState.LastPassedLevelNumber,
                AudioVolume = 1,
            };

            return new GameStateProxy(gameState, this);
        }
    }
}
