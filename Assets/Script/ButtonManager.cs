using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1;
        GameManager.Instance.ChangeState(GameState.Ingame);
        if (ArduinoManager.instance != null)
        {
            ArduinoManager.instance.TryOpenSerial();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        GameManager.Instance.RestartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
