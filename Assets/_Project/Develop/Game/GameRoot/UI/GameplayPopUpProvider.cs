using GameplayRoot;
using Zenject;

namespace UI
{
    public class GameplayPopUpProvider : PopUpProvider
    {
        private BonusSlotPopUp.Factory _bonusSlotPopUpfactory;
        private GameOverPopUp.Factory _gameOverPopUpFactory;
        private LevelCompletionPopUp.Factory _levelCompletionPopUpFactory;

        [Inject]
        private void Construct(BonusSlotPopUp.Factory bonusSlotPopUpfactory,
                               GameOverPopUp.Factory gameOverPopUpFactory,
                               LevelCompletionPopUp.Factory levelCompletionPopUpFactory)
        {
            _bonusSlotPopUpfactory = bonusSlotPopUpfactory;
            _gameOverPopUpFactory = gameOverPopUpFactory;
            _levelCompletionPopUpFactory = levelCompletionPopUpFactory;
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
    }
}
