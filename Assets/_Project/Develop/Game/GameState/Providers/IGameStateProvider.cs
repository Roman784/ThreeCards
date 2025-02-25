using R3;

namespace GameState
{
    public interface IGameStateProvider
    {
        public GameStateProxy GameState { get; }
        public GameSessionStateProvider GameSessionStateProvider { get; }

        public Observable<GameStateProxy> LoadGameState();
        public Observable<bool> SaveGameState();
        public Observable<bool> ResetGameState();
    }
}
