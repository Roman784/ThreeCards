using Gameplay;
using GameplayServices;
using UI;
using UnityEngine;
using Zenject;

namespace GameplayInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private SlotBar _slotBar;

        public override void InstallBindings()
        {
            BindUI();
            BindFactories();
            BindCardServices();
            BindSlotBar();
        }

        private void BindUI()
        {
            GameplayUI gameplayUIPrefab = Resources.Load<GameplayUI>("UI/GameplayUI");
            Container.Bind<GameplayUI>().FromInstance(gameplayUIPrefab).AsSingle();
        }

        private void BindFactories()
        {
            Card cardPrefab = Resources.Load<Card>("Prefabs/Gameplay/Card");
            Container.BindFactory<Card, CardFactory>().FromComponentInNewPrefab(cardPrefab);

            Slot slotPrefab = Resources.Load<Slot>("Prefabs/Gameplay/Slot");
            Container.BindFactory<Slot, SlotFactory>().FromComponentInNewPrefab(slotPrefab);
        }

        private void BindCardServices()
        {
            Container.Bind<CardLayoutService>().AsTransient();
            Container.Bind<CardMarkingService>().AsTransient();
            Container.Bind<CardMatchingService>().AsSingle();
        }

        private void BindSlotBar()
        {
            Container.Bind<SlotBar>().FromInstance(_slotBar).AsSingle();
        }
    }
}
