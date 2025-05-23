using Currencies;
using Gameplay;
using Settings;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameOverPopUp : PopUp
    {
        [Space]

        [SerializeField] private GameObject _bonusSlotView;
        [SerializeField] private TMP_Text _bonusSlotCostView;

        [Space]

        [SerializeField] private TMP_Text _magicStickCostView;

        private int _bonusSlotCost;
        private int _magicStickCost;

        private GameplayTools _gameplayTools;
        private ChipsCounter _chipsCounter;
        private SlotBar _slotBar;
        private PopUpProvider _popUpProvider;

        [Inject]
        private void Construct(GameplayUI gameplayUI,
                               ISettingsProvider settingsProvider,
                               ChipsCounter chipsCounter,
                               SlotBar slotBar,
                               PopUpProvider popUpProvider)
        {
            _bonusSlotCost = settingsProvider.GameSettings.SlotsSettings.BonusSlotCost;
            _magicStickCost = settingsProvider.GameSettings.ToolsSettings.MagicStickCost;

            _bonusSlotCostView.text = _bonusSlotCost.ToString();
            _magicStickCostView.text = _magicStickCost.ToString();

            _gameplayTools = gameplayUI.GameplayTools;
            _chipsCounter = chipsCounter;
            _slotBar = slotBar;
            _popUpProvider = popUpProvider;
        }

        public override void Open(bool fadeScreen = true)
        {
            base.Open(fadeScreen);
            _bonusSlotView.SetActive(!_slotBar.IsBonusSlotCreated);
        }

        public void ByeBonusSlot()
        {
            if (!CheckCost(_bonusSlotCost)) return;

            _chipsCounter.Reduce(_bonusSlotCost);
            _slotBar.CreateBonusSlot();

            Close();
        }

        public void UseMagicStick()
        {
            var result = _gameplayTools?.PickThree() ?? false;

            if (result)
                Close();
        }

        public void RestartLevel()
        {
            var result = _gameplayTools?.RestartLevel() ?? false;

            if (result)
                Close();
        }

        private bool CheckCost(int cost)
        {
            if (_chipsCounter.Count >= cost) return true;

            _popUpProvider.OpenAdvertisingChipsPopUp();
            return false;
        }

        public class Factory : PopUpFactory<GameOverPopUp>
        {
        }
    }
}
