using System.Threading.Tasks;
using UnityEngine;

namespace Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private GameSettings _gameSettings;

        public GameSettings GameSettings => _gameSettings;
        public ApplicationSettings ApplicationSettings { get; }

        public SettingsProvider()
        {
            ApplicationSettings = Resources.Load<ApplicationSettings>("ApplicationSettings");
            LoadSettings();
        }

        public Task<GameSettings> LoadGameSettings()
        {
            _gameSettings = Resources.Load<GameSettings>("GameSettings");
            return Task.FromResult(_gameSettings);
        }

        private async void LoadSettings()
        {
            await LoadGameSettings();
        }
    }
}
