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
        private PopUpProvider _popUpProvider;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider,
                               ChipsCounter chipsCounter,
                               SlotBar slotBar,
                               PopUpProvider popUpProvider)
        {
            _cost = settingsProvider.GameSettings.SlotsSettings.BonusSlotCost;
            _costView.text = _cost.ToString();

            _chipsCounter = chipsCounter;
            _slotBar = slotBar;
            _popUpProvider = popUpProvider;
        }

        public void Bye()
        {
            if (_cost > _chipsCounter.Count)
            {
                _popUpProvider.OpenAdvertisingChipsPopUp();
                return;
            }

            _chipsCounter.Reduce(_cost);
            _slotBar.CreateBonusSlot();

            Close();
        }

        public class Factory : PopUpFactory<BonusSlotPopUp>
        {
        }
    }
}
