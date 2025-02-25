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
            Container.Bind<GameplayUI>().FromComponentInNewPrefab(gameplayUIPrefab).AsSingle();

            Container.Bind<LevelProgress>().AsSingle();
        }

        private void BindFactories()
        {
            CardView cardPrefab = Resources.Load<CardView>("Prefabs/Gameplay/Card");
            Container.BindFactory<CardView, CardFactory>().FromComponentInNewPrefab(cardPrefab);

            SlotView slotPrefab = Resources.Load<SlotView>("Prefabs/Gameplay/Slot");
            Container.BindFactory<SlotView, SlotFactory>().FromComponentInNewPrefab(slotPrefab);
        }

        private void BindSlotBar()
        {
            Container.Bind<SlotBar>().FromInstance(_slotBar).AsSingle();
        }
    }
}
