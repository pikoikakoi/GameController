using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu, Ingame, GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action<GameState> OnStateChanged;
    private GameState currentState = GameState.MainMenu;

    [Header("Panel")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject InGamePanel;
    [SerializeField] private GameObject GameOverPanel;

    [Header("Audio")]
    public AudioSource bgm;
    public AudioSource deadSfx;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        bgm.Play();
        if (PlayerPrefs.GetInt("IsRestart", 0) == 1)
        {
            // Jika restart, langsung ke gameplay
            currentState = GameState.Ingame;
            OnStateChanged?.Invoke(currentState);
            PlayerPrefs.SetInt("IsRestart", 0); // Reset state restart
            StartGameWithoutHome();
        }
        else
        {
            // Jika game pertama kali, tampilkan UI Home
            ShowHomeUI();
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(newState); // Panggil event ketika state berubah

        if (newState == GameState.GameOver)
        {
            GameOver(true);
        }
    }

    public void GameOver(bool value)
    {
        deadSfx.Play();
        bgm.Stop();
        GameOverPanel.SetActive(value);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        PlayerPrefs.SetInt("IsRestart", 1);
        SceneManager.LoadScene(0);
    }

    private void ShowHomeUI()
    {
        MainMenuPanel.SetActive (true);
        InGamePanel.SetActive(false);
    }

    private void HideHomeUI()
    {
        MainMenuPanel.SetActive(false);
    }

    private void StartGameWithoutHome()
    {
        // Langsung mulai game tanpa menampilkan UI Home
        HideHomeUI();
        InGamePanel.SetActive(true);
    }
}
