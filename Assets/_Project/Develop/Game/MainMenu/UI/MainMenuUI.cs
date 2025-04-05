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

        private int _currentLevelNumber;

        [Inject]
        private void Construct(AudioPlayer audioPlayer)
        {
            _audioVolumeChanger.Construct(audioPlayer, _gameStateProvider);
        }

        public void Play()
        {
            PlayButtonClickSound();

            var enterParams = new GameplayEnterParams(_currentLevelNumber, 0);
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

            var enterParams = new LevelMenuEnterParams(_currentLevelNumber, 0);
            new SceneLoader().LoadAndRunLevelMenu(enterParams);
        }

        public void OpenRules()
        {

        }

        public void SetCurrentLevelNumber(int number)
        {
            _currentLevelNumber = number;
        }

        public void SetAudioVolumeChangerView(float volume)
        {
            _audioVolumeChanger.SetView(volume);
        }
    }
}
