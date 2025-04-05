using GameplayRoot;
using GameRoot;
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
