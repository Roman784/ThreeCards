using System.Collections;
using UnityEngine;
using Utils;

namespace ScriptAnimations
{
    public class PulsationAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _period;
        [SerializeField] private AnimationCurve _scaleChanges;
        [SerializeField] private bool _isAwake;

        private Coroutine _pulseRoutine;
        private Vector3 _originScale;

        private void Awake()
        {
            if (_isAwake)
                Pulse(_target, int.MaxValue);
        }

        private void OnDestroy()
        {
            Stop();
        }

        public void Pulse(Transform target, int rate = 1)
        {
            if (_pulseRoutine != null)
            {
                StopCoroutine(_pulseRoutine);
                _target.localScale = _originScale;
            }

            _target = target;
            _originScale = target.localScale;
            _pulseRoutine = StartCoroutine(PulseRoutine(rate));
        }

        public void Stop()
        {
            if (_pulseRoutine != null)
                StopCoroutine(_pulseRoutine);

            if (_target != null)
                _target.localScale = _originScale;
        }

        private IEnumerator PulseRoutine(int rate)
        {
            float wholeTime = _period * rate;

            for (float time = 0; time < wholeTime; time += Time.deltaTime)
            {
                _target.localScale = _originScale * _scaleChanges.Evaluate((time % _period) / _period);
                yield return null;
            }

            _target.localScale = _originScale;
        }
    }
}
