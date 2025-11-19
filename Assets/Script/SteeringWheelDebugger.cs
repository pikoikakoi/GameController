using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SteeringWheelDebugger : MonoBehaviour
{
    private InputDevice steeringDevice;
    private Rigidbody2D rb;

    [Header("Debug Display")]
    [SerializeField] private bool showDebug = true;
    [SerializeField] private bool showConsoleLog = false;

    private float steeringValue;
    private float throttleValue;

    // === Added for calibration (match PlayerMovement) ===
    private float centerX = -1f;
    private float centerY = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        FindSteeringDevice();
    }

    private void FindSteeringDevice()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device.name.Contains("Steering") ||
               (device.layout == "Joystick" && device.name.Contains("HID")))
            {
                steeringDevice = device;
                Debug.Log($"Genius Trio Racer F1 Found: {device.name}");

                var stickX = steeringDevice.TryGetChildControl<AxisControl>("stick/x");
                var stickY = steeringDevice.TryGetChildControl<AxisControl>("stick/y");

                if (stickX != null) centerX = stickX.ReadValue();
                if (stickY != null) centerY = stickY.ReadValue();

                Debug.Log($"Calibration centerX={centerX}, centerY={centerY}");
                return;
            }
        }

        Debug.LogWarning("Steering Wheel not found!");
    }

    private void Update()
    {
        if (steeringDevice == null) return;

        ReadInput();
    }

    private void ReadInput()
    {
        var stickX = steeringDevice.TryGetChildControl<AxisControl>("stick/x");
        var stickY = steeringDevice.TryGetChildControl<AxisControl>("stick/y");

        // === Steering (normalized) ===
        if (stickX != null)
        {
            float raw = stickX.ReadValue();
            steeringValue = Mathf.InverseLerp(centerX, -centerX, raw) * 2f - 1f;

            if (Mathf.Abs(steeringValue) < 0.1f)
                steeringValue = 0f;
        }

        // === Throttle (normalized) ===
        if (stickY != null)
        {
            float rawY = stickY.ReadValue();
            throttleValue = Mathf.InverseLerp(centerY, -1f, rawY);

            if (throttleValue < 0.1f)
                throttleValue = 0f;
        }

        if (showConsoleLog)
        {
            Debug.Log($"[DEBUG] Steering={steeringValue:F3} | Throttle={throttleValue:F3}");
        }
    }

    private void OnGUI()
    {
        if (!showDebug) return;

        DrawMainInfo();
        DrawSteeringBar();
        DrawThrottleBar();
        DrawDirectionIndicator();
    }

    private void DrawMainInfo()
    {
        GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
        {
            fontSize = 16,
            normal = { textColor = Color.cyan },
            padding = new RectOffset(15, 15, 15, 15),
            alignment = TextAnchor.UpperLeft
        };

        string info = "=== GENIUS TRIO RACER F1 ===\n\n";

        if (steeringDevice != null)
        {
            info += $"Device: {steeringDevice.name}\n";
            info += $"Status: CONNECTED\n\n";
            info += $"Steering: {steeringValue:F3}\n";
            info += $"Throttle: {throttleValue:F3}\n\n";

            if (rb != null)
                info += $"Velocity: ({rb.velocity.x:F2}, {rb.velocity.y:F2})";
        }
        else
        {
            info += "DEVICE NOT FOUND\n\n";
            info += "Pastikan steering wheel\nterhubung!";
        }

        GUI.Box(new Rect(10, 10, 350, 220), info, boxStyle);
    }

    private void DrawSteeringBar()
    {
        float x = 10;
        float y = 240;
        float width = 350;
        float height = 30;

        GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        GUI.Box(new Rect(x, y, width, height), "");

        GUI.color = Color.white;
        GUI.Box(new Rect(x + width * 0.5f - 1, y, 2, height), "");

        float barWidth = Mathf.Abs(steeringValue) * (width * 0.5f);
        float barX = steeringValue >= 0 ? x + width * 0.5f : x + width * 0.5f - barWidth;

        GUI.color = steeringValue > 0.1f ? Color.green :
                    steeringValue < -0.1f ? Color.yellow :
                    Color.gray;

        GUI.Box(new Rect(barX, y, barWidth, height), "");

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white },
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };

        string direction = steeringValue > 0.1f ? "➡️ KANAN" :
                           steeringValue < -0.1f ? "⬅️ KIRI" :
                           "CENTER";

        GUI.color = Color.white;
        GUI.Label(new Rect(x, y, width, height), $"STEERING: {steeringValue:F2} {direction}", labelStyle);
    }

    private void DrawThrottleBar()
    {
        float x = 10;
        float y = 280;
        float width = 350;
        float height = 30;

        GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        GUI.Box(new Rect(x, y, width, height), "");

        GUI.color = Color.white;
        GUI.Box(new Rect(x + width * 0.5f - 1, y, 2, height), "");

        float barWidth = throttleValue * (width * 0.5f);
        float barX = x + width * 0.5f;

        GUI.color = throttleValue > 0.1f ? Color.green :
                    throttleValue < -0.1f ? Color.red :
                    Color.gray;

        GUI.Box(new Rect(barX, y, barWidth, height), "");

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white },
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };

        string pedal = throttleValue > 0.1f ? "GAS" :
                      throttleValue < -0.1f ? "BRAKE" :
                      "IDLE";

        GUI.color = Color.white;
        GUI.Label(new Rect(x, y, width, height), $"THROTTLE: {throttleValue:F2} {pedal}", labelStyle);
    }

    private void DrawDirectionIndicator()
    {
        float x = 10;
        float y = 320;
        float size = 100;

        DrawCircle(x + size / 2, y + size / 2, size / 2, new Color(0.2f, 0.2f, 0.2f, 0.9f));

        Vector2 direction = new Vector2(steeringValue, throttleValue);
        if (direction.magnitude > 0.1f)
        {
            direction.Normalize();
            Vector2 center = new(x + size / 2, y + size / 2);
            Vector2 arrowEnd = center + direction * (size / 2 - 10);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GUIUtility.RotateAroundPivot(angle, center);

            GUI.color = Color.cyan;
            GUI.Box(new Rect(center.x, center.y - 2, Vector2.Distance(center, arrowEnd), 4), "");

            GUIUtility.RotateAroundPivot(-angle, center);
        }

        GUI.color = Color.white;
        DrawCircle(x + size / 2, y + size / 2, 5, Color.white);

        GUIStyle label = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white },
            fontSize = 10
        };

        GUI.Label(new Rect(x, y + size + 5, size, 20), "DIRECTION", label);
    }

    private void DrawCircle(float x, float y, float radius, Color color)
    {
        GUI.color = color;
        float diameter = radius * 2;
        GUI.Box(new Rect(x - radius, y - radius, diameter, diameter), "", GUI.skin.box);
    }
}
