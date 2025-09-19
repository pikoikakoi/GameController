using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    void OnEnable()
    {
        Bubble.CollectBubble += TakeBubble;
    }

    void OnDisable()
    {
        Bubble.CollectBubble -= TakeBubble;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void TakeBubble(float value)
    {
        currentHealth += value;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
