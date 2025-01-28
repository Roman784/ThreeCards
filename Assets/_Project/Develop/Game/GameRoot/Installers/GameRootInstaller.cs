using Settings;
using Zenject;

namespace GameRootInstallers
{
    public class GameRootInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSettingsProvider();
            BindGameState();
        }

        private void BindSettingsProvider()
        {
            var settingsProvider = new SettingsProvider();
            Container.Bind<ISettingsProvider>().FromInstance(settingsProvider).AsSingle();
        }

        private void BindGameState()
        {
            /*var gameStateProvider = new PlayerPrefsGameStateProvider();
            var gameStateProxy = new GameStateProxy(gameStateProvider.GameState);

            Container.Bind<IGameStateProvider>().To<PlayerPrefsGameStateProvider>().AsSingle();
            Container.Bind<GameStateProxy>().AsSingle();*/
        }
    }
}
