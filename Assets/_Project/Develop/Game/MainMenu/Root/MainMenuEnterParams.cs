using GameRoot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace MainMenuRoot
{
    public class MainMenuEnterParams : SceneEnterParams
    {
        public int CurrentLevelNumber { get; }

        public MainMenuEnterParams(int currentLevelNumber) : base(Scenes.MAIN_MENU)
        {
            CurrentLevelNumber = currentLevelNumber;
        }
    }
}
