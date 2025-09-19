using System.Drawing;
using TMPro;
using UnityEngine;

public class UIPoin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI points;
    private int pointValue;

    void OnEnable()
    {
        Coin.CollectCoin += UpdateUI;
    }

    void Start()
    {
        points.text = pointValue.ToString();
    }

    void Update()
    {
        points.text = pointValue.ToString();
    }

    private void UpdateUI(int value)
    {
        pointValue += value;
    }
}
