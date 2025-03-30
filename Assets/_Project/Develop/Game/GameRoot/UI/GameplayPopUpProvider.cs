using GameplayRoot;
using Utils;
using Zenject;

namespace UI
{
    public class GameplayPopUpProvider : PopUpProvider
    {
        private BonusSlotPopUp.Factory _bonusSlotPopUpfactory;
        private GameOverPopUp.Factory _gameOverPopUpFactory;
        private LevelCompletionPopUp.Factory _levelCompletionPopUpFactory;
        private BonusWhirlpoolTransitionPopUp.Factory _bonusWhirlpoolTransitionPopUpFactory;

        [Inject]
        private void Construct(BonusSlotPopUp.Factory bonusSlotPopUpfactory,
                               GameOverPopUp.Factory gameOverPopUpFactory,
                               LevelCompletionPopUp.Factory levelCompletionPopUpFactory,
                               BonusWhirlpoolTransitionPopUp.Factory bonusWhirlpoolTransitionPopUpFactory)
        {
            _bonusSlotPopUpfactory = bonusSlotPopUpfactory;
            _gameOverPopUpFactory = gameOverPopUpFactory;
            _levelCompletionPopUpFactory = levelCompletionPopUpFactory;
            _bonusWhirlpoolTransitionPopUpFactory = bonusWhirlpoolTransitionPopUpFactory;
        }

        public void OpenBonuSlotPopUp()
        {
            _bonusSlotPopUpfactory.Create().Open();
        }

        public void OpenGameOverPopUp()
        {
            _gameOverPopUpFactory.Create().Open();
        }

        public void OpenLevelCompletionPopUp(GameplayEnterParams enterParams)
        {
            _levelCompletionPopUpFactory.Create(enterParams).Open();
        }

        public void OpenBonusWhirlpoolTransitionPopUp(GameplayEnterParams enterParams, Timer timer)
        {
            _bonusWhirlpoolTransitionPopUpFactory.Create(enterParams, timer).Open();
        }
    }
}
