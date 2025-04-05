using GameplayRoot;
using GameRoot;
using LevelMenuRoot;
using UnityEngine;

namespace UI
{
    public class MainMenuUI : SceneUI
    {
        private int _currentLevelNumber;

        public void Play()
        {
            PlayButtonClickSound();

            var enterParams = new GameplayEnterParams(_currentLevelNumber, 0);
            new SceneLoader().LoadAndRunGameplay(enterParams);
        }

        public void ChangeAudioVolume()
        {
            
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
    }
}
