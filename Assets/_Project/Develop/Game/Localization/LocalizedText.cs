using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;

        [Space]

        [SerializeField] private RectTransform[] _layoutsForRebuild;

        private ILocalizationProvider _localizationProvider;

        public void Localize(ILocalizationProvider localizationProvider)
        {
            _localizationProvider = localizationProvider;

            string text = _localizationProvider.GetTranslation(_key);
            GetComponent<TMP_Text>().text = text;

            RebuildLayouts();
        }

        private void RebuildLayouts()
        {
            foreach (var layout in _layoutsForRebuild)
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
        }
    }
}
