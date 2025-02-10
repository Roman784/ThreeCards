using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines _instance;

        private static Coroutines instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            if (routine != null)
                instance.StopCoroutine(routine);
        }

        public static void StopAllRoutines()
        {
            instance.StopAllCoroutines();
        }

        public static void Invoke(Action action, float time)
        {
            instance.StartCoroutine(InvokeRoutine(action, time));
        }

        private static IEnumerator InvokeRoutine(Action action, float time)
        {
            yield return new WaitForSeconds(time);

            action.Invoke();
        }
    }
}
