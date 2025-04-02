using TMPro;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourcer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);
        }

        public void PlayOneShot(AudioClip clip)
        {
            _audioSource.loop = false;
            _audioSource.PlayOneShot(clip);
        }

        public void PlayLoop(AudioClip clip)
        {
            _audioSource.loop = true;
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}
