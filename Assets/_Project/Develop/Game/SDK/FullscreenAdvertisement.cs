using UnityEngine;
using Zenject;

namespace SDK
{
    public class FullscreenAdvertisement : MonoBehaviour
    {
        private SDK _sdk;

        [Inject]
        private void Construct(SDK sdk)
        {
            _sdk = sdk;
        }

        public void Show()
        {
            _sdk.ShowFullscreenAdv();
        }
    }
}
