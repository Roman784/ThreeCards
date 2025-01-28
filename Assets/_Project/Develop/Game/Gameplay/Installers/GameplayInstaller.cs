using GameplayServices;
using UnityEngine;
using Zenject;

namespace GameplayInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindCardServices();
        }

        private void BindCardServices()
        {
            Container.Bind<CardLayoutService>().AsSingle();
        }
    }
}
