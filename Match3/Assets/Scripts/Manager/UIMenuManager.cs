using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
    public Text LogResult;

    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) => 
        {
            if (success)
                ShowLogResult("Logged In");
            else
                ShowLogResult("Couldn't Log In");
        });
    }

    public void LogOut()
    {
        PlayGamesPlatform.Instance.SignOut();
        ShowLogResult("Logged Out");
    }

    public void GoToGameplayScene()
    {
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
}
