using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPanel;

    private void OnEnable()
    {
        Obstacle.CollectObstacle += GameOver;
    }

    private void OnDisable()
    {
        Obstacle.CollectObstacle -= GameOver;
    }

    private void GameOver(bool value)
    {
        
        GameOverPanel.SetActive(value);
        Time.timeScale = 0;
    }
}
