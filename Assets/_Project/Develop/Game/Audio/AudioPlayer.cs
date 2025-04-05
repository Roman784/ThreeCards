using DG.Tweening;
using R3;
using System;
using System.Collections;
using UnityEngine;
using Utils;
using Zenject;

namespace Audio
{
    public class AudioPlayer
    {
        private ObjectPool<AudioSourcer> _audioSourcers;
        private AudioSourcer _musicSourcer;

        private Coroutine _playingRoutine;

        [Inject]
        private void Construct(AudioSourcer audioSourcerPrefab)
        {
            _audioSourcers = new(audioSourcerPrefab, 5);
            _musicSourcer = GameObject.Instantiate(audioSourcerPrefab);
        }

        public void PlayMusic(AudioClip audioClip)
        {
            if (_musicSourcer.CurrentClip == audioClip) return;

            _musicSourcer.PlayLoop(audioClip);
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            var sourcer = _audioSourcers.GetInstance();

            sourcer.PlayOneShot(audioClip);
            DOVirtual.DelayedCall(audioClip.length, () => _audioSourcers.ReleaseInstance(sourcer));
        }

        public void PlayUntil(AudioClip audioClip, Observable<Unit> stopTrigger)
        {
            var sourcer = _audioSourcers.GetInstance();

            stopTrigger.Subscribe(_ => sourcer.Stop());
            sourcer.PlayLoop(audioClip);
        }

        public Coroutine PlayAnyTimes(AudioClip audioClip, int count, float delay, Observable<Unit> stopTrigger = null)
        {
            _playingRoutine = Coroutines.StartRoutine(PlayAnyTimesRoutine(audioClip, count, delay));
            stopTrigger?.Subscribe(_ =>
            {
                if (_playingRoutine != null)
                    Coroutines.StopRoutine(_playingRoutine);
            });

            return _playingRoutine;
        }

        private IEnumerator PlayAnyTimesRoutine(AudioClip audioClip, int count, float delay)
        {
            for (int i = 0; i < count; i++)
            {
                PlayOneShot(audioClip);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
