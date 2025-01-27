using R3;
using UnityEngine;

namespace GameplayRoot
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        public void Run(GameplayEnterParams enterParams)
        {
            Debug.Log($"Level number {enterParams.LevelNumber}");
            Debug.Log("Gameplay scene loaded");
        }
    }
}
