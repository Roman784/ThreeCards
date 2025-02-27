using DG.Tweening;
using UnityEngine;

namespace CameraUtils
{
    public class ShakyCamera : MonoBehaviour
    {
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _strength = 0.5f;
        [SerializeField] private int _vibrato = 3; 

        public void Shake()
        {
            Vector3 originalPosition = transform.localPosition;

            transform.DOShakePosition(_duration, _strength, _vibrato).OnComplete(() =>
            {
                transform.localPosition = originalPosition;
            });
        }
    }
}
