using System.Threading.Tasks;

namespace Settings
{
    public interface ISettingsProvider
    {
        public GameSettings GameSettings { get; }
        public ApplicationSettings ApplicationSettings { get; }

        public Task<GameSettings> LoadGameSettings();
    }
}
