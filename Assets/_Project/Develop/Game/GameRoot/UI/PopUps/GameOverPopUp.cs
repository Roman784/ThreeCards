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
        private AdvertisingChipsPopUp _advertisingChipsPopUp;
        private AdvertisingChipsPopUp.Factory _advertisingChipsPopUpFactroy;

        [Inject]
        private void Construct(GameplayUI gameplayUI,
                               ISettingsProvider settingsProvider,
                               ChipsCounter chipsCounter,
                               SlotBar slotBar,
                               AdvertisingChipsPopUp.Factory advertisingChipsPopUpFactroy)
        {
            _bonusSlotCost = settingsProvider.GameSettings.SlotsSettings.BonusSlotCost;
            _magicStickCost = settingsProvider.GameSettings.ToolsSettings.MagicStickCost;

            _bonusSlotCostView.text = _bonusSlotCost.ToString();
            _magicStickCostView.text = _magicStickCost.ToString();

            _gameplayTools = gameplayUI.GameplayTools;
            _chipsCounter = chipsCounter;
            _slotBar = slotBar;
            _advertisingChipsPopUpFactroy = advertisingChipsPopUpFactroy;
        }

        public override void Open()
        {
            base.Open();
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

            OpenAdvertisingChipsPopUp();
            return false;
        }

        private void OpenAdvertisingChipsPopUp()
        {
            if (_advertisingChipsPopUp == null)
                _advertisingChipsPopUp = _advertisingChipsPopUpFactroy.Create();

            _advertisingChipsPopUp.Open();
        }

        public class Factory : PopUpFactory<GameOverPopUp>
        {
        }
    }
}
