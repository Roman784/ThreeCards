using UnityEngine;
using Zenject;

namespace LevelMenu
{
    public class LevelMenuUI : MonoBehaviour
    {
        [SerializeField] private Transform _levelBlcoksContainer;

        private LevelsBlockFactory _levelsBlockFactory;

        [Inject]
        private void Construct(LevelsBlockFactory levelsBlockFactory)
        {
            _levelsBlockFactory = levelsBlockFactory;
        }

        public void CreateLevelsBlocks(int count)
        {
            for (int i = 0; i < count; i++)
            {
                CreateLevelsBlock();
            }
        }

        private void CreateLevelsBlock()
        {
            var levelsBlock = _levelsBlockFactory.Create();
            levelsBlock.Attach(_levelBlcoksContainer);
        }
    }
}
