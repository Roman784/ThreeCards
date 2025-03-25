using BonusWhirlpool;
using CameraUtils;
using Gameplay;
using GameplayServices;
using UI;
using UnityEngine;
using Zenject;

namespace BonusWhirlpoolInstaller
{
    public class BonusWhirlpoolInstaller : MonoInstaller
    {
        [SerializeField] private BonusWhirlpoolSlotBar _slotBar;
        [SerializeField] private ShakyCamera _shakyCamera;

        public override void InstallBindings()
        {
            BindUI();
            BindFactories();
            BindSlotBar();
            BindCamera();
        }

        private void BindUI()
        {
            GameplayUI gameplayUIPrefab = Resources.Load<GameplayUI>("UI/GameplayUI");
            Container.Bind<GameplayUI>().FromComponentInNewPrefab(gameplayUIPrefab).AsSingle();

            BindPopUps();
        }

        private void BindPopUps()
        {
            // var bonusSlotPopUpPrefab = Resources.Load<BonusSlotPopUp>("UI/PopUps/BonusSlotPopUp");
            // Container.BindFactory<BonusSlotPopUp, BonusSlotPopUp.Factory>().FromComponentInNewPrefab(bonusSlotPopUpPrefab);
        }

        private void BindFactories()
        {
            var cardPrefab = Resources.Load<CardView>("Prefabs/Gameplay/Card");
            Container.BindFactory<CardView, CardFactory>().FromComponentInNewPrefab(cardPrefab);

            var slotPrefab = Resources.Load<SlotView>("Prefabs/Gameplay/Slot");
            Container.BindFactory<SlotView, SlotFactory>().FromComponentInNewPrefab(slotPrefab);
        }

        private void BindSlotBar()
        {
            Container.Bind<BonusWhirlpoolSlotBar>().FromInstance(_slotBar).AsSingle();
        }

        private void BindCamera()
        {
            Container.Bind<ShakyCamera>().FromInstance(_shakyCamera).AsSingle();
        }
    }
}
