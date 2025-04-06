using GameRoot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace MainMenuRoot
{
    public class MainMenuEnterParams : SceneEnterParams
    {
        public MainMenuEnterParams(string exitSceneName) : base(Scenes.MAIN_MENU, exitSceneName)
        {
        }
    }
}
