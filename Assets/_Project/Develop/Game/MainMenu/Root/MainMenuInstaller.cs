using LevelMenu;
using UI;
using UnityEngine;
using Zenject;

namespace MainMenuRoot
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindUI();   
        }

        private void BindUI()
        {
            var mainMenuUIPrefab = Resources.Load<MainMenuUI>("UI/MainMenuUI");
            Container.Bind<MainMenuUI>().FromComponentInNewPrefab(mainMenuUIPrefab).AsSingle();
        }
    }
}
