using Audio;
using Settings;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelMenu
{
    public class LevelsBlock
    {
        private LevelsBlockView _view;
        private float _progress;

        private bool _isOpen;

        public LevelsBlock(LevelsBlockView view, Vector2Int levelNumberRange, float progress,
                           AudioPlayer audioPlayer, UIAudioSettings uiAudioSettings)
        {
            _view = view;
            _progress = progress;
            _isOpen = false;

            _view.SetLevelNumberRange(levelNumberRange);
            _view.SetProgress(_progress);

            _view.OnOpenClose += () =>
            {
                audioPlayer.PlayOneShot(uiAudioSettings.LevelBlockOpenCloseSound);
                OpenClose();
            };
        }

        public void Attach(Transform parent)
        {
            _view?.Attach(parent);
        }

        public void CreateLevelButtons(Vector2Int levelNumberRange, int lastPassedLevelNumber, LevelMenuUI levelMenu)
        {
            for (int number = levelNumberRange.x; number <= levelNumberRange.y; number++)
            {
                var isPassed = number <= lastPassedLevelNumber;
                var isLocked = number > lastPassedLevelNumber + 1;

                CreateLevelButton(number, isPassed, isLocked, levelMenu);
            }
        }

        private void OpenClose()
        {
            if (_isOpen)
                _view.Close();
            else
                _view.Open();

            _isOpen = !_isOpen;
        }

        private void CreateLevelButton(int number, bool isPassed, bool isLocked, LevelMenuUI levelMenu)
        {
            var button = _view?.CreateLevelButton(number, isPassed, isLocked, levelMenu);
            _view?.AttachButton(button);
        }
    }
}
