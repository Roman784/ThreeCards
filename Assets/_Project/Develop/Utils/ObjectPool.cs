using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private T _prefab;
        private List<T> _instances = new();

        public ObjectPool(T prefab, int initialCount)
        {
            _prefab = prefab;
            _instances.Capacity = initialCount;

            for (int i = 0; i < initialCount; i++)
            {
                CreateInstance();
            }
        }

        public T GetInstance()
        {
            var instance = _instances.FirstOrDefault(inst => !inst.isActiveAndEnabled);

            if (instance == null)
            {
                instance = CreateInstance();
            }

            instance.gameObject.SetActive(true);
            return instance;
        }

        public void ReleaseInstance(T instance)
        {
            instance.gameObject.SetActive(false);
        }

        private T CreateInstance()
        {
            var instance = Object.Instantiate(_prefab);
            instance.gameObject.SetActive(false);
            _instances.Add(instance);

            return instance;
        }
    }
}
