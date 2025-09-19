using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    public static event Action<int> CollectCoin;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectCoin?.Invoke(value);
            Destroy(gameObject);
        }
    }
}
