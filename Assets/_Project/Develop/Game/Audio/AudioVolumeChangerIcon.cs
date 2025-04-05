using UnityEngine;

namespace Audio
{
    public class AudioVolumeChangerIcon : AudioVolumeChanger
    {
        [SerializeField] private Sprite _volumeOn;
        [SerializeField] private Sprite _volumeOff;

        public override float Change()
        {
            var volume = base.Change();
            SetView(volume);

            return volume;
        }

        public override void SetView(float volume)
        {
            _iconView.sprite = volume > 0 ? _volumeOn : _volumeOff;
        }
    }
}
