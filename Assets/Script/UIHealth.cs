using UnityEngine;
using TMPro;

public class UIHealth : MonoBehaviour
{
    public TextMeshProUGUI playerHealth;
    private PlayerHealth health;

    private void Start()
    {
        health = FindFirstObjectByType<PlayerHealth>();
        playerHealth.text = "HP: " + health.GetHealth().ToString();
    }

    void Update()
    {
        playerHealth.text = "HP: " + health.GetHealth().ToString();
    }
}
