using DG.Tweening;
using UnityEngine;

namespace Audio
{
    public class AudioVolumeChangerStrikethrough : AudioVolumeChanger
    {
        [SerializeField] private Transform _line;
        [SerializeField] private float _duration;

        private Tweener _tweener;

        public override float Change()
        {
            var volume = base.Change();
            SetView(volume);

            return volume;
        }

        public override void SetView(float volume)
        {
            _tweener?.Kill();

            if (volume > 0)
                _tweener = _line.DOScaleX(0, _duration).SetEase(Ease.OutQuad);
            else
                _tweener = _line.DOScaleX(1, _duration).SetEase(Ease.OutQuad);
        }
    }
}
