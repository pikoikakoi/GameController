using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float value;
    public static event Action<float> CollectObstacle;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectObstacle?.Invoke(value);
            Destroy(gameObject);
        }
    }
}
