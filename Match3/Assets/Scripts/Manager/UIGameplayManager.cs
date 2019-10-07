using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameplayManager : MonoBehaviour
{
    public GameObject PauseButton;
    public GameObject PausePanel;
    public Controller GameController;
    public Text ScoreText;
    int LastScore;
    public AnalyticsManager AnMan;

    private void Start()
    {
        ScoreText.text = "0";
    }

    private void Update()
    {
        int score = GameController.GetScore();
        if(score!=LastScore)
        {
            LastScore = score;
            ScoreText.text = LastScore.ToString();
        }
    }

    public void GoToMenu()
    {
        AnMan.FinishLevel(GameController.GetScore());
#if UNITY_ANDROID && !UNITY_EDITOR
        GPSManager.Instance.UploadScore(GameController.GetScore());
        GPSManager.Instance.ShowLeaderboard();
#endif
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        PauseButton.SetActive(false);
        PausePanel.SetActive(true);
        GameController.Pause();
    }

    public void UnpauseGame()
    {
        PauseButton.SetActive(true);
        PausePanel.SetActive(false);
        GameController.Unpause();
    }
}
