using Audio;
using GameplayRoot;
using GameRoot;
using LevelMenuRoot;
using UnityEngine;
using Utils;
using Zenject;

namespace UI
{
    public class MainMenuUI : SceneUI
    {
        [SerializeField] private AudioVolumeChanger _audioVolumeChanger;

        private SDK.SDK _sdk;

        [Inject]
        private void Construct(AudioPlayer audioPlayer, SDK.SDK sdk)
        {
            _audioVolumeChanger.Construct(audioPlayer, _gameStateProvider);
            
            _sdk = sdk;
        }

        public void Play()
        {
            _sdk.ShowFullscreenAdv();

            PlayButtonClickSound();

            var enterParams = new GameplayEnterParams(Scenes.MAIN_MENU, GetCurrentLevelNumber(), 0);
            new SceneLoader().LoadAndRunGameplay(enterParams);
        }

        public void ChangeAudioVolume()
        {
            PlayButtonClickSound();

            _audioVolumeChanger.Change();
        }

        public void OpenLevelMenu()
        {
            PlayButtonClickSound();

            var enterParams = new LevelMenuEnterParams(Scenes.MAIN_MENU, GetCurrentLevelNumber(), 0);
            new SceneLoader().LoadAndRunLevelMenu(enterParams);
        }

        public void OpenRules()
        {
            PlayButtonClickSound();
            _popUpProvider.OpenGameRulesPopUp();
        }

        public void OpenLanguagePopUp()
        {
            PlayButtonClickSound();
            _popUpProvider.OpenLanguagePopUp();
        }

        public void SetAudioVolumeChangerView(float volume)
        {
            _audioVolumeChanger.SetView(volume);
        }

        private int GetCurrentLevelNumber()
        {
            var levelNumber = _gameStateProvider.GameState.LastPassedLevelNumber.Value + 1;
            return _settingsProvider.GameSettings.CardLayoutsSettings.ClampLevelNumber(levelNumber);
        }
    }
}
