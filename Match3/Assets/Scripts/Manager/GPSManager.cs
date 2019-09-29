using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GPSManager : MonoBehaviourSingleton<GPSManager>
{
    bool LoggedIn;

    private void Start()
    {
        InitializeGooglePlayServices();
    }

    void InitializeGooglePlayServices()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    public void ShowAchievements()
    {
        if(LoggedIn)
            Social.ShowAchievementsUI();
    }

    public void ShowLeaderboard()
    {
        if(LoggedIn)
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_highscore);
    }

    public void UploadScore(int score)
    {
        if (LoggedIn)
        {
            Social.ReportScore(score, GPGSIds.leaderboard_highscore, (bool success) =>
            {
                if (success)
                    ShowLeaderboard();
            });
        }
    }

    public void UnlockAchievement()
    {
        if (LoggedIn)
        {
            Social.ReportProgress(GPGSIds.achievement_campen_mundial_de_match3, 100.0f, (bool success) =>
            {
                if (success)
                    ShowAchievements();
            });
        }
    }

    public void SetLoggedIn(bool log)
    {
        LoggedIn = log;
    }

    public bool GetLoggedIn()
    {
        return LoggedIn;
    }
}
