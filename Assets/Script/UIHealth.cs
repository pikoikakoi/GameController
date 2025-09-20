using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider hpSlider;

    private void Start()
    {
        if (hpSlider != null && playerHealth != null)
        {
            hpSlider.maxValue = playerHealth.GetMaxHealth();
            hpSlider.value = playerHealth.GetCurrentHealth();
        }
    }

    private void Update() {
        if (hpSlider != null && playerHealth)
        {
            hpSlider.value = playerHealth.GetCurrentHealth(); 
        }
    }

}
