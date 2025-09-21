using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private string highScoreFilePath;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI bestPerformanceTxt;
    [SerializeField] private TextMeshProUGUI starCollected;
    private float highScore;

    private void Start()
    {
        highScoreFilePath = Path.Combine(Application.persistentDataPath, "highscore.dat");
        LoadHighscore();
    }

    void OnEnable()
    {
        GameManager.OnStateChanged += StateHandler;
    }

    void OnDisable()
    {
        GameManager.OnStateChanged += StateHandler;
    }

    private void Update()
    {
        Timer.Instance.GetTime();
    }

    private void StateHandler(GameState newState)
    {
        if (newState == GameState.GameOver)
        {
            CheckHighscore();
        }
    }

    private void SetTime(float elapsedTime, TextMeshProUGUI targetText, string prefix = "")
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        targetText.text = prefix + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateUI()
    {
        if (bestPerformanceTxt != null) SetTime(highScore, bestPerformanceTxt); // "Best Performance: "
        if (starCollected != null) starCollected.text = UIPoin.Instance.GetPoints().ToString(); // "Stars Collected: "
        if (timerText != null) SetTime(Timer.Instance.GetTime(), timerText);    // "Your Time: "
    }

    private void LoadHighscore()
    {
        if (File.Exists(highScoreFilePath)) //cek file highscore
        {
            BinaryFormatter formatter = new BinaryFormatter();  //membuat objek binary formatter
            using (FileStream file = File.Open(highScoreFilePath, FileMode.Open))   // membuka file dalam mode baca
            {
                highScore = (float)formatter.Deserialize(file); // membaca dan mengonversi data dari binary ke float
            }
        }
        else
        {
            highScore = 0;
        }
    }

    private void CheckHighscore()
    {
        if (Timer.Instance.GetTime() > highScore)
        {
            highScore = Timer.Instance.GetTime();
            SaveHighscore();
        }
        UpdateUI();
    }

    private void SaveHighscore()
    {
        BinaryFormatter formatter = new BinaryFormatter();  //membuat objek binaryformatter
        using (FileStream file = File.Create(highScoreFilePath))    //membuka atau membuat file untuk menyimpan data
        {
            formatter.Serialize(file, highScore);   //menyimpan data highscore ke file dengan format binary
        }
    }
}
