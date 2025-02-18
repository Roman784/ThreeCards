using System.Collections;
using UnityEngine;
using Utils;

namespace ScriptAnimations
{
    public class PulsationAnimator : MonoBehaviour
    {
        [SerializeField] private float _period;
        [SerializeField] private AnimationCurve _scaleChanges;

        private Coroutine _pulseRoutine;
        private Transform _target;
        private Vector3 _originScale;

        public void Pulse(Transform target, int rate = 1)
        {
            if (_pulseRoutine != null)
            {
                Coroutines.StopRoutine(_pulseRoutine);
                _target.localScale = _originScale;
            }

            _target = target;
            _originScale = target.localScale;
            _pulseRoutine = Coroutines.StartRoutine(PulseRoutine(rate));
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
