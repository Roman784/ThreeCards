using System;

namespace UI
{
    public class LevelProgress
    {
        private LevelProgressView _view;
        public void BindView(LevelProgressView view)
        {
            _view = view;
        }

        public void SetLevelNumber(int levelNumber)
        {
            _view.SetLevelNumber(levelNumber);
        }

        public void FillProgressBar(float progress)
        {
            if (progress < 0 || progress > 1)
                throw new ArgumentOutOfRangeException("Progress cannot be greater than 1 or less than 0.");

            _view.FillProgressBar(progress);
        }
    }
}
