using TMPro;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourcer : MonoBehaviour
    {
        private AudioSource _audioSource;

        public AudioClip CurrentClip { get; private set; }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);
        }

        public void PlayOneShot(AudioClip clip)
        {
            CurrentClip = clip;

            _audioSource.loop = false;
            _audioSource.PlayOneShot(clip);
        }

        public void PlayLoop(AudioClip clip)
        {
            CurrentClip = clip;

            _audioSource.loop = true;
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void Stop()
        {
            CurrentClip = null;
            _audioSource.Stop();
        }
    }
}
