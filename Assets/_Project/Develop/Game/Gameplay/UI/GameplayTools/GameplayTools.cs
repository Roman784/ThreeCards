using GameplayServices;

namespace UI
{
    public class GameplayTools
    {
        private GameplayToolsView _view;
        private bool _isEnabled;

        private FieldShufflingService _fieldShufflingService;

        public void BindView(GameplayToolsView view)
        {
            _view = view;
            _view.Disable();

            _view.OnShuffleField += () => ShuffleField();
        }

        public void Init(FieldShufflingService fieldShufflingService)
        {
            _fieldShufflingService = fieldShufflingService;
        }

        public void Enable()
        {
            _isEnabled = true;
            _view.Enable();
        }

        public void Disable()
        {
            _isEnabled = false;
            _view.Disable();
        }

        private void ShuffleField()
        {
            if (!_isEnabled) return;
            _fieldShufflingService.Shuffle();
        }
    }
}
