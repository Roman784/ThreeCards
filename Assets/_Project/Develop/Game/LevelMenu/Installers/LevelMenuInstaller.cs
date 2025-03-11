using Gameplay;
using GameplayServices;
using LevelMenu;
using UI;
using UnityEngine;
using Zenject;

namespace LevelMenuInstallers
{
    public class LevelMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindUI();
            BindFactories();
        }

        private void BindUI()
        {
            var levelMenuUIPrefab = Resources.Load<LevelMenuUI>("UI/LevelMenuUI");
            Container.Bind<LevelMenuUI>().FromComponentInNewPrefab(levelMenuUIPrefab).AsSingle();
        }

        private void BindFactories()
        {
            var levelsBlockPrefab = Resources.Load<LevelsBlockView>("UI/Levels/LevelsBlock");
            Container.BindFactory<LevelsBlockView, LevelsBlockFactory>().FromComponentInNewPrefab(levelsBlockPrefab);
        }
    }
}
