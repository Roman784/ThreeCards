using Audio;
using GameplayRoot;
using GameRoot;
using LevelMenuRoot;
using UnityEngine;
using Zenject;

namespace UI
{
    public class MainMenuUI : SceneUI
    {
        [SerializeField] private AudioVolumeChanger _audioVolumeChanger;

        [Inject]
        private void Construct(AudioPlayer audioPlayer)
        {
            _audioVolumeChanger.Construct(audioPlayer, _gameStateProvider);
        }

        public void Play()
        {
            PlayButtonClickSound();

            var enterParams = new GameplayEnterParams(GetCurrentLevelNumber(), 0);
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

            var enterParams = new LevelMenuEnterParams(GetCurrentLevelNumber(), 0);
            new SceneLoader().LoadAndRunLevelMenu(enterParams);
        }

        public void OpenRules()
        {

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
