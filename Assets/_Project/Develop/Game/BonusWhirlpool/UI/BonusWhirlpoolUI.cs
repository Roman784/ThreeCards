using BonusWhirlpool;
using Gameplay;
using GameplayRoot;
using GameRoot;
using R3;
using Settings;
using System;
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

        private List<WhirlpoolCard> _cards;
        private Subject<Card> _pickCardSubj = new();

        private Timer _timer;
        private int _currentLevelNumber;

        [Inject]
        private void Construct()
        {
            var settings = _settingsProvider.GameSettings.BonusWhirlpoolSettings;
            var cardPickDelay = settings.CardPickDelay;
            var timerValue = settings.TimerValue;
            var timerValueOffset = settings.TimerValueOffset;

            _pickCardSubj.ThrottleFirst(TimeSpan.FromSeconds(cardPickDelay)).Subscribe(c => c.Pick());
            _timer = new Timer(Randomizer.GetRandomRange(timerValue, timerValueOffset));
        }

        private void OnDestroy()
        {
            _timer.Stop();
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
        public void SetCards(List<WhirlpoolCard> cards) => _cards = cards;

        public Observable<Unit> StartTimer()
        {
            return _timer.Start();
        }

        public void BackToCurrentLevel()
        {
            OpenLevel(_currentLevelNumber, 0f);
        }
    
        public void PickCard()
        {
            _pickCardSubj.OnNext(_cards[0].Card);
        }
    }
}
