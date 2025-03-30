using BonusWhirlpoolRoot;
using GameplayRoot;
using GameRoot;
using TMPro;
using UnityEngine;
using Utils;
using R3;
using DG.Tweening;

namespace UI
{
    public class BonusWhirlpoolTransitionPopUp : PopUp
    {
        [SerializeField] private CanvasGroup _goToViewGroup;
        [SerializeField] private CanvasGroup _timerViewGroup;
        [SerializeField] private TMP_Text _timerView;

        private GameplayEnterParams _gameplayEnterParams;
        private Timer _timer;
        private CompositeDisposable _disposable = new();


        public override void Open(bool fadeScreen = true)
        {
            _timerViewGroup.alpha = 0f;
            _goToViewGroup.alpha = 1f;

            base.Open(fadeScreen);
        }
        public override void Close(bool appearScreen = true)
        {
            _disposable.Dispose();
            base.Close(appearScreen);
        }

        public void SetGameplayEnterParams(GameplayEnterParams enterParams)
        {
            _gameplayEnterParams = enterParams;
        }

        public void SetTimer(Timer timer)
        {
            _timer = timer;
            _timer.OnValueChanged.Subscribe(value => UpdateView(value)).AddTo(_disposable);
        }

        public void OpenBonusLevel()
        {
            if (_timer.Value > 0) return;

            var enterParams = new BonusWhirlpoolEnterParams(_gameplayEnterParams.LevelNumber);
            new SceneLoader().LoadAndRunBonusWhirlpool(enterParams);
        }

        private void UpdateView(float time)
        {
            RenderTime(time);

            if (time <= 0)
            {
                _timerViewGroup.DOFade(0f, 0.25f).SetEase(Ease.OutQuad);
                _goToViewGroup.DOFade(1f, 0.25f).SetEase(Ease.InQuad);
            }
            else
            {
                _timerViewGroup.alpha = 1f;
                _goToViewGroup.alpha = 0f;
            }
        }

        public void RenderTime(float time)
        {
            var minutes = (int)(time / 60f);
            var secs = (int)(time % 60f);

            var formattedTime = string.Format("{0:00}:{1:00}",
                minutes,
                secs);

            _timerView.text = formattedTime;
        }

        public class Factory : PopUpFactory<BonusWhirlpoolTransitionPopUp>
        {
            public BonusWhirlpoolTransitionPopUp Create(GameplayEnterParams gameplayEnterParams, Timer timer)
            {
                var popUp = base.Create();
                popUp.SetGameplayEnterParams(gameplayEnterParams);
                popUp.SetTimer(timer);

                return popUp;
            }
        }
    }
}
