using GameplayRoot;
using GameRoot;
using System.Collections;

namespace LevelMenuRoot
{
    public class LevelMenuEntryPoint : SceneEntryPoint
    {
        public override IEnumerator Run<T>(T enterParams)
        {
            yield return Run(enterParams.As<LevelMenuEnterParams>());
        }

        private IEnumerator Run(LevelMenuEnterParams enterParams)
        {
            yield return null;
        }
    }
}
