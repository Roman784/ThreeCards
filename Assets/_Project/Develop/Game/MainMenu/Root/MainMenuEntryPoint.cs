using GameRoot;
using GameState;
using LevelMenu;
using LevelMenuRoot;
using System.Collections;
using UI;
using UnityEngine;
using Zenject;
using R3;
using Settings;
using Audio;

namespace MainMenuRoot
{
    public class MainMenuEntryPoint : SceneEntryPoint
    {
        private IGameStateProvider _gameStateProvider;
        private ISettingsProvider _settingsProvider;
        private SDK.SDK _sdk;
        private UIRootView _uiRoot;
        private MainMenuUI _mainMenuUI;
        private AudioPlayer _audioPlayer;

        [Inject]
        private void Construct(IGameStateProvider gameStateProvider,
                               ISettingsProvider settingsProvider,
                               SDK.SDK sdk,
                               UIRootView uiRoot,
                               MainMenuUI mainMenuUI,
                               AudioPlayer audioPlayer)
        {
            _gameStateProvider = gameStateProvider;
            _settingsProvider = settingsProvider;
            _sdk = sdk;
            _uiRoot = uiRoot;
            _mainMenuUI = mainMenuUI;
            _audioPlayer = audioPlayer;
        }

        public override IEnumerator Run<T>(T enterParams)
        {
            yield return Run(enterParams.As<MainMenuEnterParams>());
        }

        private IEnumerator Run(MainMenuEnterParams enterParams)
        {
            var isLoaded = false;

            _gameStateProvider.LoadGameState().Subscribe(_ =>
            {
                // Settings.
                var audioSettings = _settingsProvider.GameSettings.AudioSettings;

                // Audio.
                var volume = _gameStateProvider.GameState.AudioVolume.Value;
                _audioPlayer.SetVolume(volume);
                _audioPlayer.PlayMusic(audioSettings.MusicSettings.MainTheme);

                // UI.
                _uiRoot.AttachSceneUI(_mainMenuUI.gameObject);
                _mainMenuUI.SetAudioVolumeChangerView(volume);

                _mainMenuUI.LocalizeTexts();

                // SDK.
                _sdk.GameReady();

                isLoaded = true;
            });

            yield return new WaitUntil(() => isLoaded);
        }
    }
}
