using TMPro;
using UnityEngine;

public class UIPoin : MonoBehaviour
{
    public static UIPoin Instance;
    [SerializeField] private TextMeshProUGUI points;
    private int pointValue;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    
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

    public int GetPoints()
    {
        return pointValue;
    }
}
