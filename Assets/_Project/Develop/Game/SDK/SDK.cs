using R3;
using System;
using UnityEngine;

namespace SDK
{
    public abstract class SDK : MonoBehaviour
    {
        private bool _isGameStopped;
        private float _currentSoundVolume;

        public abstract Observable<bool> Init();
        public abstract void SaveData(string data);
        public abstract Observable<string> LoadData();
        public abstract Observable<bool> ShowRewardedVideo();
        public abstract void ShowFullscreenAdv();
        public abstract string GetLanguage();
        public abstract void GameReady();

        public void StopGame()
        {
            if (_isGameStopped) return;
            _isGameStopped = true;

            _currentSoundVolume = AudioListener.volume;

            AudioListener.volume = 0;
            Time.timeScale = 0f;
        }

        public void ContinueGame()
        {
            if (!_isGameStopped) return;
            _isGameStopped = false;

            AudioListener.volume = _currentSoundVolume;
            Time.timeScale = 1f;
        }
    }
}
