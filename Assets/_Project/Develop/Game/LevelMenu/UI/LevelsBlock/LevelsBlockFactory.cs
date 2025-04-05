using Audio;
using Settings;
using UnityEngine;
using Zenject;

namespace LevelMenu
{
    public class LevelsBlockFactory : PlaceholderFactory<LevelsBlockView>
    {
        private AudioPlayer _audioPlayer;
        private UIAudioSettings _uiAudioSettings;

        [Inject]
        private void Construct(AudioPlayer audioPlayer, ISettingsProvider settingsProvider)
        {
            _audioPlayer = audioPlayer;
            _uiAudioSettings = settingsProvider.GameSettings.AudioSettings.UIAudioSettings;
        }

        public LevelsBlock Create(Vector2Int levelNumberRange, float progress)
        {
            var view = base.Create();
            return new LevelsBlock(view, levelNumberRange, progress, _audioPlayer, _uiAudioSettings);
        }
    }
}
