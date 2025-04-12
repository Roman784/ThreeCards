mergeInto(LibraryManager.library, {

    InitYSDKExtern : function(id)
    {
        initSDK(id);
    },

    SaveDataExtern : function(date)
    {
        var dateString = UTF8ToString(date);
        var myobj = JSON.parse(dateString);
        player.setData(myobj);
        console.log('Save data');
    },

    LoadDataExtern : function()
    {
        player.getData().then(_date => {
            const myJSON = JSON.stringify(_date);
            gameInstance.SendMessage('YandexSDK', 'AcceptLoadedData', myJSON);
            console.log('Load data');
        });
    },

    ShowRewardedVideoExtern : function(id) 
    {
        ysdk.adv.showRewardedVideo({
            callbacks: {
                onOpen: () => {
                    gameInstance.SendMessage('YandexSDK', 'StopGame');
                    console.log('Video ad open.');
                },
                onRewarded: () => {
                    gameInstance.SendMessage('YandexSDK', 'InvokeCallback', id);
                    console.log('Rewarded!');
                },
                onClose: () => {
                    gameInstance.SendMessage('YandexSDK', 'ContinueGame');
                    console.log('Video ad closed.');
                }, 
                onError: (e) => {
                    gameInstance.SendMessage('YandexSDK', 'ContinueGame');
                    console.log('Error while open video ad:', e);
                }
            }
        })
    },

    ShowFullscreenAdvExtern : function () 
    {
        ysdk.adv.showFullscreenAdv({
            callbacks: {
                onClose: function(wasShown) {
                    gameInstance.SendMessage('YandexSDK', 'ContinueGame');
                    console.log ("adv close");
                },
                onOpen: function(open) {
                    gameInstance.SendMessage('YandexSDK', 'StopGame');
                    console.log ("adv open");
                },
                onError: function(error) {
                    gameInstance.SendMessage('YandexSDK', 'ContinueGame');
                    console.log ("adv error");
                }
            }
        })
    },

    GetLanguageExtern: function () 
    {
        var lang = ysdk.environment.i18n.lang;
        var bufferSize = lengthBytesUTF8(lang) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(lang, buffer, bufferSize);

        return buffer;
    },

    GameReadyExtern: function ()
    {
        gameReady();
    },
});