using Currencies;
using Gameplay;
using GameplayServices;
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
            BindCurrencies();
            BindUI();
        }

        private void BindSettingsProvider()
        {
            Container.Bind<ISettingsProvider>().To<SettingsProvider>().AsSingle();
        }

        private void BindGameStateProvider()
        {
            Container.Bind<IGameStateProvider>().To<PlayerPrefsGameStateProvider>().AsSingle();
        }

        private void BindCurrencies()
        {
            Container.Bind<ChipsCounter>().AsSingle();
        }

        private void BindUI()
        {
            UIRootView uiRootPrefab = Resources.Load<UIRootView>("UI/UIRoot");
            Container.Bind<UIRootView>().FromComponentInNewPrefab(uiRootPrefab).AsSingle();

            BindPopUps();
        }

        private void BindPopUps()
        {
            SettingsPopUp settingsPopUpPrefab = Resources.Load<SettingsPopUp>("UI/PopUps/SettingsPopUp");
            Container.BindFactory<SettingsPopUp, SettingsPopUp.Factory>().FromComponentInNewPrefab(settingsPopUpPrefab);
        }
    }
}
