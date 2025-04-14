using Currencies;
using SDK;
using Zenject;
using R3;

namespace UI
{
    public class AdvertisingChipsPopUp : PopUp
    {
        private ChipsCounter _chipsCounter;
        private SDK.SDK _sdk;
        private PopUpProvider _popUpProvider;

        [Inject]
        private void Construct(ChipsCounter chipsCounter, SDK.SDK sdk, PopUpProvider popUpProvider)
        {
            _chipsCounter = chipsCounter;
            _sdk = sdk;
            _popUpProvider = popUpProvider;
        }

        public void WatchVideo()
        {
            _sdk.ShowRewardedVideo().Subscribe(res =>
            {
                if (res)
                    GetChips();
                else
                    _popUpProvider.OpenAdvertisingErrorPopUp();
            });
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
