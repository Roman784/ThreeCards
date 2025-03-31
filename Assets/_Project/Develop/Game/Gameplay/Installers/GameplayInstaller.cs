using CameraUtils;
using Gameplay;
using GameplayServices;
using GameState;
using UI;
using UnityEngine;
using Zenject;

namespace GameplayInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private SlotBar _slotBar;
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

            Container.Bind<LevelProgress>().AsSingle();
            Container.Bind<GameplayTools>().AsSingle();
            Container.Bind<BonusWhirlpoolTransition>().AsSingle();

            Container.Bind<GameplayPopUpProvider>().AsTransient();

            BindPopUps();
        }

        private void BindPopUps()
        {
            var bonusSlotPopUpPrefab = Resources.Load<BonusSlotPopUp>("UI/PopUps/BonusSlotPopUp");
            Container.BindFactory<BonusSlotPopUp, BonusSlotPopUp.Factory>().FromComponentInNewPrefab(bonusSlotPopUpPrefab);

            var gameOverPopUpPrefab = Resources.Load<GameOverPopUp>("UI/PopUps/GameOverPopUp");
            Container.BindFactory<GameOverPopUp, GameOverPopUp.Factory>().FromComponentInNewPrefab(gameOverPopUpPrefab);

            var levelCompletionPopUpPrefab = Resources.Load<LevelCompletionPopUp>("UI/PopUps/LevelCompletionPopUp");
            Container.BindFactory<LevelCompletionPopUp, LevelCompletionPopUp.Factory>()
                .FromComponentInNewPrefab(levelCompletionPopUpPrefab);

            var bonusWhirlpoolTransitionPopUpPrefab = Resources
                .Load<BonusWhirlpoolTransitionPopUp>("UI/PopUps/BonusWhirlpoolTransitionPopUp");
            Container.BindFactory<BonusWhirlpoolTransitionPopUp, BonusWhirlpoolTransitionPopUp.Factory>()
                .FromComponentInNewPrefab(bonusWhirlpoolTransitionPopUpPrefab);
        }

        private void BindFactories()
        {
            var cardPrefab = Resources.Load<CardView>("Prefabs/Gameplay/Card");
            Container.BindFactory<CardView, CardFactory>().FromComponentInNewPrefab(cardPrefab);

            var slotPrefab = Resources.Load<SlotView>("Prefabs/Gameplay/Slots/Slot");
            Container.BindFactory<SlotView, SlotFactory>().FromComponentInNewPrefab(slotPrefab);
        }

        private void BindSlotBar()
        {
            Container.Bind<SlotBar>().FromInstance(_slotBar).AsSingle();
        }

        private void BindCamera()
        {
            Container.Bind<ShakyCamera>().FromInstance(_shakyCamera).AsSingle();
        }
    }
}
