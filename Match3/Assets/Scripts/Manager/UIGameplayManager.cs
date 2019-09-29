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

    public void GoToMenu()
    {
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
