using Currencies;
using Zenject;

namespace UI
{
    public class AdvertisingChipsPopUp : PopUp
    {
        private ChipsCounter _chipsCounter;

        [Inject]
        private void Construct(ChipsCounter chipsCounter)
        {
            _chipsCounter = chipsCounter;
        }

        public void WatchVideo()
        {
            GetChips();
        }

        private void GetChips()
        {
            _chipsCounter.Add(300, true, false, true);
            Close();
        }

        public class Factory : PopUpFactory<AdvertisingChipsPopUp>
        {
        }
    }
}
