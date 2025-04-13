using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SDK
{
    public class YandexSDK : SDK
    {
        private const string SDK_NAME = nameof(YandexSDK);

        [DllImport("__Internal")] private static extern void InitYSDKExtern(int id);
        [DllImport("__Internal")] private static extern void SaveDataExtern(string date);
        [DllImport("__Internal")] private static extern void LoadDataExtern();
        [DllImport("__Internal")] private static extern void ShowRewardedVideoExtern(int id);
        [DllImport("__Internal")] private static extern void ShowFullscreenAdvExtern();
        [DllImport("__Internal")] private static extern string GetLanguageExtern();
        [DllImport("__Internal")] private static extern void GameReadyExtern();

        private Dictionary<int, Subject<bool>> _callbacksMap = new();
        private Subject<string> _jsonDataCallback;

        private void Awake()
        {
            gameObject.name = SDK_NAME;
        }

        public override Observable<bool> Init()
        {
            try
            {
                var callback = new Subject<bool>();
                var id = RegisterCallback(callback);

                InitYSDKExtern(id);

                return callback;
            }
            catch 
            {
                Debug.LogError("SDK initialization error!");
                return Observable.Return(false);
            }
        }

        public override void SaveData(string data)
        {
            try { SaveDataExtern(data); }
            catch { Debug.LogError("Save extern error!"); }
        }

        public override Observable<string> LoadData()
        {
            try
            {
                LoadDataExtern();
                return _jsonDataCallback;
            }
            catch 
            { 
                Debug.LogError("Load extern error!");
                return Observable.Return("none");
            }
        }

        public void AcceptLoadedData(string json)
        {
            _jsonDataCallback.OnNext(json);
        }

        public override Observable<bool> ShowRewardedVideo()
        {
            try
            {
                var callback = new Subject<bool>();
                var id = RegisterCallback(callback);

                ShowRewardedVideoExtern(id);

                return callback;
            }
            catch 
            {
                Debug.LogError("Rewarded video error!");
                return Observable.Return(false);
            }
        }

        public override void ShowFullscreenAdv()
        {
            try { ShowFullscreenAdvExtern(); }
            catch { Debug.LogError("Full screen adv error"); }
        }

        public override string GetLanguage()
        {
            try { return GetLanguageExtern(); }
            catch { return "en"; }
        }

        public override void GameReady()
        {
            GameReadyExtern();
        }

        public void InvokeCallback(int id)
        {
            _callbacksMap[id].OnNext(true);
            _callbacksMap.Remove(id);
        }

        private int RegisterCallback(Subject<bool> callback)
        {
            var id = _callbacksMap.OrderByDescending(item => item.Key)?.First().Key + 1 ?? 0;
            _callbacksMap.Add(id, callback);

            return id;
        }
    }
}
