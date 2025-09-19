using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    [SerializeField] private TextMeshProUGUI timerText;
    private float elapsedTime;
    private float collectTime;
    private bool isPlaying = false;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    private void OnEnable()
    {
        ButtonManager.StartGame += CheckInGame;
        Obstacle.CollectObstacle += SaveTimer;
    }

    private void OnDisable()
    {
        ButtonManager.StartGame -= CheckInGame;
        Obstacle.CollectObstacle -= SaveTimer;
    }

    private void CheckInGame(bool isPlay)
    {
        isPlaying = isPlay;
    }

    private void Update()
    {
        if (!isPlaying) return;
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SaveTimer(bool value)
    {
        if (!value) return;
        collectTime = elapsedTime;
    }

    public float GetTime()
    {
        return collectTime;
    }
}
