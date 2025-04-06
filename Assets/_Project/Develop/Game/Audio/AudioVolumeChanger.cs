using GameState;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Audio
{
    public abstract class AudioVolumeChanger : MonoBehaviour
    {
        protected AudioPlayer _audioPlayer;
        private IGameStateProvider _gameStateProvider;

        public void Construct(AudioPlayer audioPlayer, IGameStateProvider gameStateProvider)
        {
            _audioPlayer = audioPlayer;
            _gameStateProvider = gameStateProvider;
        }

        public virtual float Change()
        {
            float volume;

            if (_audioPlayer.Volume > 0)
                volume = _audioPlayer.SetVolume(0);
            else
                volume = _audioPlayer.SetVolume(1);

            _gameStateProvider.GameState.AudioVolume.Value = volume;

            return volume;
        }

        public abstract void SetView(float volume);
    }
}
