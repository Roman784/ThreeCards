using DG.Tweening;
using Gameplay;
using GameplayRoot;
using GameRoot;
using UnityEngine;

namespace UI
{
    public class LevelCompletionPopUp : PopUp
    {
        [Space]

        [SerializeField] private float _initialDelay;

        private GameplayEnterParams _gameplayEnterParams;

        public void SetGameplayEnterParams(GameplayEnterParams gameplayEnterParams)
        {
            _gameplayEnterParams = gameplayEnterParams;
        }

        public override void Open(bool fadeScreen = false)
        {
            DOVirtual.DelayedCall(_initialDelay, () =>
            {
                base.Open(false);
            });
        }

        public void GoToNextLevel()
        {
            PlayButtonClickSound();
            if (_gameplayEnterParams != null)
                new SceneLoader().LoadAndRunGameplay(_gameplayEnterParams);

            Close();
        }

        public class Factory : PopUpFactory<LevelCompletionPopUp>
        {
            public LevelCompletionPopUp Create(GameplayEnterParams gameplayEnterParams)
            {
                var popUp = base.Create();
                popUp.SetGameplayEnterParams(gameplayEnterParams);

                return popUp;
            }
        }
    }
}
