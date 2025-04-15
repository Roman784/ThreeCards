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
        [SerializeField] private YandexSDK _yandexSdkPrefab;
        [SerializeField] private EditorSDK _editorSdkPrefab;

        [Space]

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
#if UNITY_EDITOR
            Container.Bind<SDK.SDK>().FromComponentInNewPrefab(_editorSdkPrefab).AsSingle().NonLazy();
#else
            Container.Bind<SDK.SDK>().FromComponentInNewPrefab(_sdkPrefab).AsSingle().NonLazy();
            Container.Bind<FullscreenAdvertisement>().AsSingle();
#endif
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
#if UNITY_EDITOR
            Container.Bind<IGameStateProvider>().To<PlayerPrefsGameStateProvider>().AsSingle();
#else
            Container.Bind<IGameStateProvider>().To<SDKGameStateProvider>().AsSingle();
#endif
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

            var advertisingChipsPopUpPrefab = Resources.Load<AdvertisingChipsPopUp>("UI/PopUps/Advertising/AdvertisingChipsPopUp");
            Container.BindFactory<AdvertisingChipsPopUp, AdvertisingChipsPopUp.Factory>().FromComponentInNewPrefab(advertisingChipsPopUpPrefab);

            var advertisingErrorPopUpPrefab = Resources.Load<AdvertisingErrorPopUp>("UI/PopUps/Advertising/AdvertisingErrorPopUp");
            Container.BindFactory<AdvertisingErrorPopUp, AdvertisingErrorPopUp.Factory>().FromComponentInNewPrefab(advertisingErrorPopUpPrefab);

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
