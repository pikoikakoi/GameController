using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    void OnEnable()
    {
        Bubble.CollectBubble += TakeBubble;
        Obstacle.CollectObstacle += TakeDamage;
    }

    void OnDisable()
    {
        Bubble.CollectBubble -= TakeBubble;
        Obstacle.CollectObstacle -= TakeDamage;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void TakeBubble(float value)
    {
        currentHealth += value;
    }

    private void TakeDamage(float value)
    {
        currentHealth -= value;
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
