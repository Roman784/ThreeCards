using DG.Tweening;
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
        private bool _canExit;

        public void SetGameplayEnterParams(GameplayEnterParams gameplayEnterParams)
        {
            _gameplayEnterParams = gameplayEnterParams;
        }

        public override void Open(bool fadeScreen = true)
        {
            base.Open(fadeScreen);
            DOVirtual.DelayedCall(1.5f, () => _canExit = true);
        }

        public override void Close(bool appearScreen = true)
        {
            if (!_canExit) return;

            base.Close(appearScreen);
            BackToLevel();
        }

        public void BackToLevel()
        {
            if (!_canExit) return;

            var enterParams = new GameplayEnterParams(_gameplayEnterParams.LevelNumber, 
                                                      _gameplayEnterParams.BonusWhirlpoolTimerValue);
            new SceneLoader().LoadAndRunGameplay(enterParams);
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
