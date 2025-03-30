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
        private GameplayEnterParams _gameplayEnterParams;

        public void SetGameplayEnterParams(GameplayEnterParams gameplayEnterParams)
        {
            _gameplayEnterParams = gameplayEnterParams;
        }

        public void BackToLevel()
        {
            var enterParams = new GameplayEnterParams(_gameplayEnterParams.LevelNumber, 
                                                      _gameplayEnterParams.BonusWhirlpoolTimerValue);
            new SceneLoader().LoadAndRunGameplay(enterParams);
        }

        public override void Close(bool appearScreen = true)
        {
            base.Close(appearScreen);
            BackToLevel();
        }

        public class Factory : PopUpFactory<BonusWhirlpoolTimeOverPopUp>
        {
            public BonusWhirlpoolTimeOverPopUp Create(GameplayEnterParams gameplayEnterParams)
            {
                var popUp = base.Create();
                popUp.SetGameplayEnterParams(gameplayEnterParams);

                return popUp;
            }
        }
    }
}
