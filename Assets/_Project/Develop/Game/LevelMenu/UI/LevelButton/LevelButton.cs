using Audio;
using Settings;
using System;
using UnityEngine;

namespace LevelMenu
{
    public class LevelButton
    {
        private LevelButtonView _view;

        private int _number;
        private bool _isLocked;
        private LevelMenuUI _levelMenu;

        public LevelButton(LevelButtonView view, int number, bool isPassed, bool isLocked, LevelMenuUI levelMenu,
                           AudioPlayer audioPlayer, UIAudioSettings uiAudioSettings)
        {
            _view = view;
            _number = number;
            _isLocked = isLocked;
            _levelMenu = levelMenu;

            _view.SetNumber(_number);
            _view.Fill(isPassed);
            _view.Lock(isLocked);

            _view.OnOpenLevel += () =>
            {
                audioPlayer.PlayOneShot(uiAudioSettings.ButtonClickSound);
                OpenLevel();
            };
        }

        public void Attach(Transform parent)
        {
            _view?.Attach(parent);
        }

        public void OpenLevel()
        {
            if (_isLocked) return;
            _levelMenu?.OpenLevel(_number);
        }
    }
}
