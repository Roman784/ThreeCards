using System.Collections;
using UnityEngine;

namespace GameRoot
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        public abstract IEnumerator Run<T>(T enterParams) where T : SceneEnterParams;
    }
}
