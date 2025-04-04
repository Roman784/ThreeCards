using R3;
using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Currencies
{
    public class CurrencyCollectionAnimation : MonoBehaviour
    {
        [SerializeField] private CurrencyParticle _currencyPrefab;
        [SerializeField] private Transform _instancesContainer;

        [Space]

        [SerializeField] private float _delayBetweenPrefabCreation;
        [SerializeField] private float _flightSpeed;
        [SerializeField] private AnimationCurve _rescalingDuringFlight;

        private ObjectPool<CurrencyParticle> _currenciesPool;

        private Subject<Unit> _collectedSubj = new();
        private Subject<Unit> _allCollectedSubj = new();

        public Observable<Unit> OnCollected => _collectedSubj;
        public Observable<Unit> OnAllCollected => _allCollectedSubj;

        private void Awake()
        {
            _currenciesPool = new(_currencyPrefab, 10);
        }

        public Observable<Unit> StartCollecting(int count, Vector3 from, Vector3 to)
        {
            _collectedSubj = new();

            StartCoroutine(CollectionRoutine(count, from, to));

            return OnCollected;
        }

        private IEnumerator CollectionRoutine(int count, Vector3 from, Vector3 to)
        {
            for (int i = 0; i < count; i++)
            {
                var newInstance = _currenciesPool.GetInstance();
                newInstance.transform.SetParent(_instancesContainer, false);

                var remaining = count - i - 1;
                Action onFlewInAction = () => DestroyCurrencyInstance(newInstance, remaining);

                StartCoroutine(FlyRoutine(newInstance.transform, from, to, onFlewInAction));

                yield return new WaitForSeconds(_delayBetweenPrefabCreation);
            }
        }

        private IEnumerator FlyRoutine(Transform target, Vector3 from, Vector3 to, Action onFlewIn)
        {
            float wholeDistance = Vector3.Distance(from, to);
            float remainingDistance = wholeDistance;

            target.position = from;
            while (remainingDistance > 0.05f)
            {
                remainingDistance = Vector3.Distance(target.position, to);

                target.localScale = Vector3.one * _rescalingDuringFlight.Evaluate((wholeDistance - remainingDistance) / wholeDistance);
                target.position = Vector3.MoveTowards(target.position, to, _flightSpeed * Time.deltaTime);

                yield return null;
            }
            target.position = to;

            onFlewIn?.Invoke();
        }

        private void DestroyCurrencyInstance(CurrencyParticle instance, int remaining)
        {
            _currenciesPool.ReleaseInstance(instance);
            _collectedSubj.OnNext(Unit.Default);

            if (remaining <= 0)
                _allCollectedSubj.OnNext(Unit.Default);
        }
    }
}
