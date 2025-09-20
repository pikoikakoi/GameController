using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float decreaseTime;
    private bool isPlaying;

    void OnEnable()
    {
        Bubble.CollectBubble += TakeBubble;
        GameManager.OnStateChanged += StateHandler;
    }

    void OnDisable()
    {
        Bubble.CollectBubble -= TakeBubble;
        GameManager.OnStateChanged -= StateHandler;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void StateHandler(GameState newState)
    {
        if (newState == GameState.Ingame)
        {
            isPlaying = true;
        }
    }

    private void Update()
    {
        if (!isPlaying) return;
        // Health berkurang seiring waktu
        currentHealth -= decreaseTime * Time.deltaTime;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        CheckHealth();
    }

    private void TakeBubble(float value)
    {
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
