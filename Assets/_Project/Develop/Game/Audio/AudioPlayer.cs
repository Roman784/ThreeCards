using DG.Tweening;
using UnityEngine;
using Utils;
using Zenject;

namespace Audio
{
    public class AudioPlayer
    {
        private ObjectPool<AudioSourcer> _audioSourcers;

        [Inject]
        private void Construct(AudioSourcer audioSourcerPrefab)
        {
            _audioSourcers = new(audioSourcerPrefab, 5);
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            var sourcer = _audioSourcers.GetInstance();

            sourcer.PlayOneShot(audioClip);
            DOVirtual.DelayedCall(audioClip.length, () => _audioSourcers.ReleaseInstance(sourcer));
        }
    }
}
