using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;
using System;

namespace Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key;
        [SerializeField] private TMP_Text _view;

        [Space]

        [SerializeField] private RectTransform[] _layoutsForRebuild;

        private ILocalizationProvider _localizationProvider;
        private IDisposable _disposable;

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }

        public void Localize(ILocalizationProvider localizationProvider)
        {
            _localizationProvider = localizationProvider;
            _disposable = _localizationProvider.OnLanguageChanged.Subscribe(_ => SetView());

            SetView();
        }

        private void SetView()
        {
            string text = _localizationProvider.GetTranslation(_key);
            if (_view != null) _view.text = text;

            RebuildLayouts();
        }

        private void RebuildLayouts()
        {
            foreach (var layout in _layoutsForRebuild)
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
        }
    }
}
