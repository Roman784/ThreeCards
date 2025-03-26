using GameplayRoot;
using GameRoot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class BonusWhirlpoolUI : SceneUI
    {
        private int _currentLevelNumber;

        public void SetCurrentLevelNumber(int levelNumber) => _currentLevelNumber = levelNumber;

        public void BackToCurrentLevel()
        {
            OpenLevel(_currentLevelNumber);
        }

        public void OpenLevel(int number)
        {
            var gameplayEnterParams = new GameplayEnterParams(number);
            new SceneLoader().LoadAndRunGameplay(gameplayEnterParams);
        }
    }
}
