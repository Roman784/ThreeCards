using GameplayRoot;
using GameRoot;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BonusWhirlpoolTimeOverPopUp : PopUp
    {
        private int _currentLevelNumber;

        public void SetCurrentLevelNumber(int currentLevelNumber)
        {
            _currentLevelNumber = currentLevelNumber;
        }

        public void BackToLevel()
        {
            var enterParams = new GameplayEnterParams(_currentLevelNumber);
            new SceneLoader().LoadAndRunGameplay(enterParams);
        }

        public override void Close(bool appearScreen = true)
        {
            base.Close(appearScreen);
            BackToLevel();
        }

        public class Factory : PopUpFactory<BonusWhirlpoolTimeOverPopUp>
        {
            public BonusWhirlpoolTimeOverPopUp Create(int currentLevelNumber)
            {
                var popUp = base.Create();
                popUp.SetCurrentLevelNumber(currentLevelNumber);

                return popUp;
            }
        }
    }
}
