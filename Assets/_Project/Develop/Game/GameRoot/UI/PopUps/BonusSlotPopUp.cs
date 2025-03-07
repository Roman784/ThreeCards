using Currencies;
using Gameplay;
using Settings;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BonusSlotPopUp : PopUp
    {
        [SerializeField] private TMP_Text _costView;
        private int _cost;

        private ChipsCounter _chipsCounter;
        private SlotBar _slotBar;
        private AdvertisingChipsPopUp _advertisingChipsPopUp;
        private AdvertisingChipsPopUp.Factory _advertisingChipsPopUpFactroy;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider,
                               ChipsCounter chipsCounter,
                               SlotBar slotBar,
                               AdvertisingChipsPopUp.Factory advertisingChipsPopUpFactroy)
        {
            _cost = settingsProvider.GameSettings.SlotsSettings.BonusSlotCost;
            _costView.text = _cost.ToString();

            _chipsCounter = chipsCounter;
            _slotBar = slotBar;
            _advertisingChipsPopUpFactroy = advertisingChipsPopUpFactroy;
        }

        public void Bye()
        {
            if (_cost > _chipsCounter.Count)
            {
                OpenAdvertisingChipsPopUp();
                return;
            }

            _chipsCounter.Reduce(_cost);
            _slotBar.CreateBonusSlot();

            Close();
        }

        private void OpenAdvertisingChipsPopUp()
        {
            if (_advertisingChipsPopUp == null)
                _advertisingChipsPopUp = _advertisingChipsPopUpFactroy.Create();

            _advertisingChipsPopUp.Open();
        }

        public class Factory : PopUpFactory<BonusSlotPopUp>
        {
        }
    }
}
