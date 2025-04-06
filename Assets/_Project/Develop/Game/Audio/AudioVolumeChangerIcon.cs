using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class AudioVolumeChangerIcon : AudioVolumeChanger
    {
        [SerializeField] protected Image _iconView;
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
