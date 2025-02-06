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
            BindSlotBar();
        }

        private void BindUI()
        {
            GameplayUI gameplayUIPrefab = Resources.Load<GameplayUI>("UI/GameplayUI");
            Container.Bind<GameplayUI>().FromInstance(gameplayUIPrefab).AsSingle();
        }

        private void BindFactories()
        {
            CardView cardPrefab = Resources.Load<CardView>("Prefabs/Gameplay/Card");
            Container.BindFactory<CardView, CardFactory>().FromComponentInNewPrefab(cardPrefab);

            Slot slotPrefab = Resources.Load<Slot>("Prefabs/Gameplay/Slot");
            Container.BindFactory<Slot, SlotFactory>().FromComponentInNewPrefab(slotPrefab);
        }

        private void BindSlotBar()
        {
            Container.Bind<SlotBar>().FromInstance(_slotBar).AsSingle();
        }
    }
}
