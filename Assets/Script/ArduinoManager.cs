using UnityEngine;
using System.Globalization;
using System.IO.Ports;

public class ArduinoManager : MonoBehaviour
{
    public static bool UseArduinoInput { get; private set; } = false;

    private string portName = "COM9";
    private int boundRate = 9600;

    SerialPort serial;

    public Vector3 accelData;

    public bool isConnected = false;

    public static ArduinoManager instance;
    private void Awake()
    {
        // singleton standar
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // opsional, kalau mau tetap hidup antar scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        TryOpenSerial();
    }

    public void TryOpenSerial()
    {
        // 1. Bersihkan koneksi lama sebelum mencoba membuka yang baru
        CloseSerial(); 
        
        // 2. Inisialisasi object SerialPort baru
        serial = new SerialPort(portName, boundRate); 
        serial.ReadTimeout = 200; // Ditingkatkan ke 200ms untuk toleransi timeout

        try
        {
            serial.Open();
            isConnected = true;
            UseArduinoInput = true;
            Debug.Log(" Serial Connected. Arduino Input ENABLED.");
        }
        catch (System.Exception e)
        {
            // Error jika COM salah/Serial Monitor terbuka
            Debug.LogError($"Gagal membuka serial port! Pastikan COM/BaudRate benar dan Serial Monitor TERTUTUP. Error: {e.Message}");
            isConnected = false;
            UseArduinoInput = false;
        }
    }

    public void CloseSerial()
    {
        if (serial != null)
        {
            if (serial.IsOpen)
            {
                serial.Close();
                Debug.Log("Serial Port Closed.");
            }
            
            // PENTING: Dispose object untuk membebaskan resource OS (Port COM)
            serial.Dispose();
            serial = null; // Set ke null untuk menghindari NRE

            isConnected = false;
            UseArduinoInput = false;
        }
    }

    private void Update()
    {
        // Cek serial object sebelum akses IsOpen (Menghindari NRE pada baris 83)
        if (serial == null || !serial.IsOpen) 
        {
            isConnected = false;
            // Debug.Log("Mati wok"); // Tidak perlu cetak setiap frame
            return;
        }

        // Serial Terbuka
        try
        {
            string line = serial.ReadLine();
            string[] values = line.Trim().Split(','); 

            if (values.Length == 3)
            {
                // Parsing aman: Gunakan CultureInfo.InvariantCulture
                float x = float.Parse(values[0], CultureInfo.InvariantCulture);
                float y = float.Parse(values[1], CultureInfo.InvariantCulture);
                float z = float.Parse(values[2], CultureInfo.InvariantCulture);

                accelData = new Vector3(x, y, z);
            }
        }
        catch (System.Exception e)
        {
            // Timeout adalah pengecualian yang paling umum di sini
            Debug.LogWarning("Serial Read Error: " + e.Message);
        }
    }
     
    public void DetectArduino()
    {
        if (!isConnected)
        {
            Debug.Log("Cant detect arduiono");
            return;
        }

        // ControllerConfigManager.instance.EnableAllButtonExcept(2);
        // GameManager.instance.ControllerSelected();

    }

    public float GetXValue() { return accelData.x; }
    public float GetYValue() { return accelData.y; }
    public float GetZValue() { return accelData.z; }

    void OnApplicationQuit()
    {
        CloseSerial();
    }

}