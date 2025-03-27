using GameplayRoot;
using GameRoot;
using R3;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Zenject;

namespace UI
{
    public class BonusWhirlpoolUI : SceneUI
    {
        [SerializeField] private TimerView _timerView;

        private Timer _timer;
        private int _currentLevelNumber;

        [Inject]
        private void Construct()
        {
            var timerValue = _settingsProvider.GameSettings.BonusWhirlpoolSettings.TimerValue;
            var timerValueOffset = _settingsProvider.GameSettings.BonusWhirlpoolSettings.TimerValueOffset;
            
            _timer = new Timer(Randomizer.GetRandomRange(timerValue, timerValueOffset));
        }

        public override void BindViews()
        {
            base.BindViews();

            _timer.BindView(_timerView);
        }

        public void SetCurrentLevelNumber(int levelNumber)
        {
            _currentLevelNumber = levelNumber;
        }

        public Observable<Unit> StartTimer()
        {
            return _timer.Start();
        }

        public void BackToCurrentLevel()
        {
            OpenLevel(_currentLevelNumber);
        }
    }
}
