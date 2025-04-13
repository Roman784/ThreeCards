using Audio;
using Currencies;
using Gameplay;
using GameplayServices;
using GameState;
using Localization;
using SDK;
using Settings;
using UI;
using UnityEngine;
using Zenject;

namespace GameRootInstallers
{
    public class GameRootInstaller : MonoInstaller
    {
        [SerializeField] private YandexSDK _sdkPrefab;
        [SerializeField] private AudioSourcer _audioSourcerPrefab;

        public override void InstallBindings()
        {
            BindSDK();
            BindLocalizationProvider();
            BindSettingsProvider();
            BindGameStateProvider();
            BindCurrencies();
            BindUI();
            BindAudioPlayer();
        }

        private void BindSDK()
        {
            Container.Bind<SDK.SDK>().FromComponentInNewPrefab(_sdkPrefab).AsSingle().NonLazy();
        }

        private void BindLocalizationProvider()
        {
            Container.Bind<ILocalizationProvider>().To<CSVLocalizationProvider>().AsSingle();
        }

        private void BindSettingsProvider()
        {
            Container.Bind<ISettingsProvider>().To<SettingsProvider>().AsSingle();
        }

        private void BindGameStateProvider()
        {
            // Container.Bind<IGameStateProvider>().To<PlayerPrefsGameStateProvider>().AsSingle();
            Container.Bind<IGameStateProvider>().To<SDKGameStateProvider>().AsSingle();
        }

        private void BindCurrencies()
        {
            Container.Bind<ChipsCounter>().AsSingle();
        }

        private void BindUI()
        {
            UIRootView uiRootPrefab = Resources.Load<UIRootView>("UI/UIRoot");
            Container.Bind<UIRootView>().FromComponentInNewPrefab(uiRootPrefab).AsSingle();

            Container.Bind<PopUpProvider>().AsTransient();

            BindPopUps();
        }

        private void BindPopUps()
        {
            var settingsPopUpPrefab = Resources.Load<SettingsPopUp>("UI/PopUps/SettingsPopUp");
            Container.BindFactory<SettingsPopUp, SettingsPopUp.Factory>().FromComponentInNewPrefab(settingsPopUpPrefab);

            var gameRulesPopUpPrefab = Resources.Load<GameRulesPopUp>("UI/PopUps/GameRulesPopUp");
            Container.BindFactory<GameRulesPopUp, GameRulesPopUp.Factory>().FromComponentInNewPrefab(gameRulesPopUpPrefab);

            var advertisingChipsPopUpPrefab = Resources.Load<AdvertisingChipsPopUp>("UI/PopUps/AdvertisingChipsPopUp");
            Container.BindFactory<AdvertisingChipsPopUp, AdvertisingChipsPopUp.Factory>().FromComponentInNewPrefab(advertisingChipsPopUpPrefab);

            var languagePopUpPrefab = Resources.Load<LanguagePopUp>("UI/PopUps/LanguagePopUp");
            Container.BindFactory<LanguagePopUp, LanguagePopUp.Factory>().FromComponentInNewPrefab(languagePopUpPrefab);
        }

        private void BindAudioPlayer()
        {
            Container.Bind<AudioSourcer>().FromInstance(_audioSourcerPrefab).AsTransient();
            Container.Bind<AudioPlayer>().AsSingle();
        }
    }
}
