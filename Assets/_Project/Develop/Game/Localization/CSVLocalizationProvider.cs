using R3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Localization
{
    public class CSVLocalizationProvider : ILocalizationProvider
    {
        private const string TRANSLATIONS_FILE_NAME = "translations";

        private static Dictionary<string, string> _translationsMap;

        public Observable<Unit> LoadTranslations(string language)
        {
            var csvData = Resources.Load<TextAsset>(TRANSLATIONS_FILE_NAME);

            var lines = csvData.text.Split('\n');
            var headers = lines[0].Split(';');

            var languageColumnIndex = -1;
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Trim() == language)
                {
                    languageColumnIndex = i;
                    break;
                }
            }

            if (languageColumnIndex == -1)
                throw new System.Exception("Language not found in CSV!");

            _translationsMap = new();
            for (int i = 1; i < lines.Length; i++)
            {
                var columns = lines[i].Split(';');
                if (columns.Length >= 2)
                {
                    var key = columns[0].Trim();
                    var value = columns[languageColumnIndex].Trim();

                    _translationsMap.Add(key, value);
                }
            }

            return Observable.Return(Unit.Default);
        }

        public string GetTranslation(string key)
        {
            if (_translationsMap.ContainsKey(key))
            {
                return _translationsMap[key];
            }
            else
            {
                Debug.LogError($"Key '{key}' not found!");
                return $"MISSING: {key}";
            }
        }
    }
}
