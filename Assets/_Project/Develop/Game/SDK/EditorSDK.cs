using R3;

namespace SDK
{
    public class EditorSDK : SDK
    {
        public override void GameReady() { }

        public override string GetLanguage()
        {
            return "en";
        }

        public override Observable<bool> Init()
        {
            return Observable.Return(true);
        }

        public override Observable<string> LoadData()
        {
            return Observable.Return("{}");
        }

        public override void SaveData(string data) { }

        public override void ShowFullscreenAdv() { }

        public override Observable<bool> ShowRewardedVideo()
        {
            return Observable.Return(true);
        }
    }
}
