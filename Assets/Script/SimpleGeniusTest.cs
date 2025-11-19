using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Testing sederhana untuk Genius Trio Racer F1
/// Cukup drag-and-drop ke GameObject dan jalankan
/// </summary>
public class SimpleGeniusTest : MonoBehaviour
{
    private void Update()
    {
        // Baca SEMUA joystick/gamepad yang terdeteksi
        var gamepad = Gamepad.current;
        var joystick = Joystick.current;
        
        if (gamepad != null)
        {
            Vector2 stick = gamepad.leftStick.ReadValue();
            if (stick.magnitude > 0.01f)
            {
                Debug.Log($"[GAMEPAD] Left Stick: X={stick.x:F3}, Y={stick.y:F3}");
            }
        }
        
        if (joystick != null)
        {
            Vector2 stick = joystick.stick.ReadValue();
            if (stick.magnitude > 0.01f)
            {
                Debug.Log($"[JOYSTICK] Stick: X={stick.x:F3}, Y={stick.y:F3}");
            }
        }
        
        // Coba baca dari device apapun yang ada
        foreach (var device in InputSystem.devices)
        {
            if (device.name.Contains("HID") || 
                device.name.Contains("Steering") ||
                device.name.Contains("Joystick"))
            {
                // Coba baca axis X dan Y
                var x = device.TryGetChildControl<UnityEngine.InputSystem.Controls.AxisControl>("x");
                var y = device.TryGetChildControl<UnityEngine.InputSystem.Controls.AxisControl>("y");
                
                if (x != null && Mathf.Abs(x.ReadValue()) > 0.01f)
                {
                    Debug.Log($"[{device.name}] X: {x.ReadValue():F3}");
                }
                
                if (y != null && Mathf.Abs(y.ReadValue()) > 0.01f)
                {
                    Debug.Log($"[{device.name}] Y: {y.ReadValue():F3}");
                }
                
                // Coba stick/x dan stick/y
                var stickX = device.TryGetChildControl<UnityEngine.InputSystem.Controls.AxisControl>("stick/x");
                var stickY = device.TryGetChildControl<UnityEngine.InputSystem.Controls.AxisControl>("stick/y");
                
                if (stickX != null && Mathf.Abs(stickX.ReadValue()) > 0.01f)
                {
                    Debug.Log($"[{device.name}] stick/x: {stickX.ReadValue():F3}");
                }
                
                if (stickY != null && Mathf.Abs(stickY.ReadValue()) > 0.01f)
                {
                    Debug.Log($"[{device.name}] stick/y: {stickY.ReadValue():F3}");
                }
            }
        }
    }
    
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;
        style.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 10, 500, 30), "GERAKKAN SETIR DAN PEDAL GENIUS TRIO RACER!", style);
        GUI.Label(new Rect(10, 40, 500, 30), "Lihat Console untuk output nilai.", style);
    }
}