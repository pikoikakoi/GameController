using System;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float value;
    public static event Action<float> CollectBubble;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectBubble?.Invoke(value);
            Destroy(gameObject);
        }
    }
}
