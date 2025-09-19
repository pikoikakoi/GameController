using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    private PlayerHealth health;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float sliderTimer;

    private void Start()
    {
        health = FindFirstObjectByType<PlayerHealth>();
        SetSlider();
    }

    private void SetSlider()
    {
        hpSlider.maxValue = health.GetMaxHealth();
        hpSlider.value = health.GetHealth();
    }

    private void StartTimer()
    {
        
    }

}
