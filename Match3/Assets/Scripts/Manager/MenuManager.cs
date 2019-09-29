using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Text LogResult;

    public void LogIn()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Social.localUser.Authenticate((bool success) => 
        {
            if (success)
            {
                ShowLogResult("Logged In");
                GPSManager.Instance.SetLoggedIn(true);
            }
            else
                ShowLogResult("Couldn't Log In");
        });
#endif
    }

    public void LogOut()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (GPSManager.Instance.GetLoggedIn())
        {
            PlayGamesPlatform.Instance.SignOut();
            ShowLogResult("Logged Out");
        }
        else
        {
            ShowLogResult("Not Logged in");
        }
#endif
    }

    public void GoToGameplayScene()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        GPSManager.Instance.UnlockAchievement();
#endif
        SceneManager.LoadScene(1);
    }

    void ShowLogResult(string result)
    {
        LogResult.gameObject.SetActive(true);
        LogResult.text = result;
        if(!IsInvoking())
            Invoke("HideLogResult", 2f);
        else
        {
            CancelInvoke();
            Invoke("HideLogResult", 2f);
        }
    }

    void HideLogResult()
    {
        LogResult.gameObject.SetActive(false);
    }

    public void OpenLeaderboard()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        GPSManager.Instance.ShowLeaderboard();
#endif
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

}
