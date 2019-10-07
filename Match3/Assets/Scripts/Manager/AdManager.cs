using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AdManager : MonoBehaviour
{
    private string GameIdAndroid = "3317309";
    private string VideoKey = "video";
    private string RewardedKey = "rewardedVideo";
    public AnalyticsManager AnMan;

    private void Awake()
    {
        Advertisement.Initialize(GameIdAndroid, true);
    }

    public void UIWatchAd()
    {
        WatchVideoAd(VideoAdEnded);
    }

    public void UIWatchRewardedAd()
    {
        WatchRewardedVideoAd(VideoRewardedAdEnded);
    }

    public void WatchVideoAd(Action<ShowResult> result)
    {
        ShowOptions so = new ShowOptions();
        so.resultCallback = result;

        if (Advertisement.IsReady(VideoKey))
            Advertisement.Show(VideoKey, so);
        else
            Debug.Log("No carga, master");
    }

    public void WatchRewardedVideoAd(Action<ShowResult> result)
    {
        ShowOptions so = new ShowOptions();
        so.resultCallback = result;

        if (Advertisement.IsReady(RewardedKey))
            Advertisement.Show(RewardedKey, so);
        else
            Debug.Log("No carga, master");
    }

    public void VideoAdEnded(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Failed:
                Debug.Log("El Ad fallo");
                break;
            case ShowResult.Finished:
                Debug.Log("El Ad termino");
                break;
            case ShowResult.Skipped:
                Debug.Log("El Ad se skipeo");
                break;
        }
    }

    public void VideoRewardedAdEnded(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                Debug.Log("El Ad Rewarded fallo");
                AnMan.SawAd(false);
                break;
            case ShowResult.Finished:
                Debug.Log("El Ad Rewarded termino");
                AnMan.SawAd(true);
                break;
            case ShowResult.Skipped:
                Debug.Log("El Ad Rewarded se skipeo");
                break;
        }
    }
}
