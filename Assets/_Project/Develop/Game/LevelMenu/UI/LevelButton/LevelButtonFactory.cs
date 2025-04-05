using Audio;
using Settings;
using Zenject;

namespace LevelMenu
{
    public class LevelButtonFactory : PlaceholderFactory<LevelButtonView>
    {
        private AudioPlayer _audioPlayer;
        private UIAudioSettings _uiAudioSettings;

        [Inject]
        private void Construct(AudioPlayer audioPlayer, ISettingsProvider settingsProvider)
        {
            _audioPlayer = audioPlayer;
            _uiAudioSettings = settingsProvider.GameSettings.AudioSettings.UIAudioSettings;
        }

        public LevelButton Create(int number, bool isPassed, bool isLocked, LevelMenuUI levelMenu)
        {
            var view = base.Create();
            return new LevelButton(view, number, isPassed, isLocked, levelMenu, _audioPlayer, _uiAudioSettings);
        }
    }
}
