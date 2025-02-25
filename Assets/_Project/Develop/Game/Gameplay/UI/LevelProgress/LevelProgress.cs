using GameplayServices;
using System;
using R3;
using UnityEngine;

namespace UI
{
    public class LevelProgress
    {
        private LevelProgressView _view;

        private int _totalCardCount;
        private int _currentCardCount;

        public void BindView(LevelProgressView view)
        {
            _view = view;
        }

        public void SetLevelNumber(int levelNumber)
        {
            _view.SetLevelNumber(levelNumber);
        }

        public void InitProgressBar(int totalCardCount, CardMatchingService cardMatchingService)
        {
            _totalCardCount = totalCardCount;
            _currentCardCount = totalCardCount;

            cardMatchingService.OnCardsRemoved.Subscribe(removedCards =>
            {
                _currentCardCount -= removedCards.Count;
                if (_currentCardCount < 0) _currentCardCount = 0;
                var progress = 1f - (float)_currentCardCount / (float)_totalCardCount;

                FillProgressBar(progress);
            });
        }

        private void FillProgressBar(float progress)
        {
            if (progress < 0 || progress > 1)
                throw new ArgumentOutOfRangeException("Progress cannot be greater than 1 or less than 0.");

            _view.FillProgressBar(progress);
        }
    }
}
