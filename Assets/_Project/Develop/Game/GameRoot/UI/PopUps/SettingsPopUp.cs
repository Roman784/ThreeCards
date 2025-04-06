using Audio;
using GameState;
using UnityEngine;
using Zenject;

namespace UI
{
    public class SettingsPopUp : PopUp
    {
        [SerializeField] private AudioVolumeChangerStrikethrough _audioVolumeChanger;

        private PopUpProvider _popUpProvider;

        [Inject]
        private void Construct(PopUpProvider popUpProvider, AudioPlayer audioPlayer, IGameStateProvider gameStateProvider)
        {
            _popUpProvider = popUpProvider;

            _audioVolumeChanger.Construct(audioPlayer, gameStateProvider);
        }

        public override void Open(bool fadeScreen = true)
        {
            var volume = _audioPlayer.Volume;
            _audioVolumeChanger.SetView(volume);

            base.Open(fadeScreen);
        }

        public void OpenGameRules()
        {
            PlayButtonClickSound();
            _popUpProvider.OpenGameRulesPopUp();
        }

        public void ChangeAudioVolume()
        {
            PlayButtonClickSound();
            _audioVolumeChanger.Change();
        }

        public class Factory : PopUpFactory<SettingsPopUp>
        {
        }
    }
}
