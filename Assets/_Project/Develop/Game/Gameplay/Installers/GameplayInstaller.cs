using Gameplay;
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
            Card cardPrefab = Resources.Load<Card>("Prefabs/Gameplay/Card");
            Container.BindFactory<Card, CardFactory>().FromComponentInNewPrefab(cardPrefab);

            Container.Bind<CardLayoutService>().AsTransient();
        }
    }
}
