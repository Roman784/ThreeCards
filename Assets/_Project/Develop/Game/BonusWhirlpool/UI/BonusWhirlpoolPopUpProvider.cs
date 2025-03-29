using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BonusWhirlpoolPopUpProvider : PopUpProvider
    {
        private BonusWhirlpoolTimeOverPopUp.Factory _timeOverPopUpFactory;

        [Inject]
        private void Construct(BonusWhirlpoolTimeOverPopUp.Factory timeOverPopUpFactroy)
        {
            _timeOverPopUpFactory = timeOverPopUpFactroy;
        }

        public void OpenTimeOverPopUp(int currentLevelNumber)
        {
            _timeOverPopUpFactory.Create(currentLevelNumber).Open();
        }
    }
}
