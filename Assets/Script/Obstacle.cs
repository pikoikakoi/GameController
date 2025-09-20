using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // [SerializeField] private float value;
    public static event Action<bool> CollectObstacle;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectObstacle?.Invoke(true);
            GameManager.Instance.ChangeState(GameState.GameOver);
            Destroy(gameObject);
        }
    }
}
