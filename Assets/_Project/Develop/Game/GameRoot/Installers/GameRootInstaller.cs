using GameState;
using Settings;
using UI;
using UnityEngine;
using Zenject;

namespace GameRootInstallers
{
    public class GameRootInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSettingsProvider();
            BindGameStateProvider();
            BindUI();
        }

        private void BindSettingsProvider()
        {
            var settingsProvider = new SettingsProvider();
            Container.Bind<ISettingsProvider>().FromInstance(settingsProvider).AsSingle();
        }

        private void BindGameStateProvider()
        {
            var gameStateProvider = new PlayerPrefsGameStateProvider();
            Container.Bind<IGameStateProvider>().FromInstance(gameStateProvider).AsSingle();
        }

        private void BindUI()
        {
            UIRootView uiRootPrefab = Resources.Load<UIRootView>("UI/UIRoot");
            Container.Bind<UIRootView>().FromComponentInNewPrefab(uiRootPrefab).AsSingle();
        }
    }
}
