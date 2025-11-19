using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum ControlDevice
{
    Keyboard, Mouse, Gamepad, SteeringWheel
}

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isMove = false;

    //player input
    private PlayerInput playerInput;

    //input keyboard
    private Vector2 moveInput;
    [SerializeField] private float speed;

    //input mouse
    private Vector2 pointerInput;
    [SerializeField] private float followSpeed = 5f;
    private Camera mainCam;

    //input steering wheel (Genius Trio Racer F1)
    [Header("Steering Wheel Settings")]
    [SerializeField] private float steerSpeed = 5f;      // Kecepatan belok kiri-kanan
    [SerializeField] private float verticalSpeed = 5f;  // Kecepatan gas/brake (atas-bawah)
    [SerializeField] private float maxSpeed = 8f;       // Batas kecepatan maksimal
    [SerializeField] private float deadzone = 0.15f;     // Dead zone untuk menghindari drift
    private float centerX = -1f; 
    private float centerY = 1f;

    // Direct device reading untuk Genius Trio Racer
    private InputDevice steeringDevice;
    private bool useDirectInput = true; // Gunakan pembacaan langsung dari device

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        sprite.enabled = false;

        mainCam = Camera.main;

        // Cari steering wheel device
        FindSteeringWheelDevice();
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += StateHandler;
        playerInput.onControlsChanged += OnControlsChanged;
    }

    void OnDisable()
    {
        GameManager.OnStateChanged -= StateHandler;
        playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void StateHandler(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                isMove = false;
            break;

            case GameState.Ingame:
                isMove = true;
                sprite.enabled = true;
            break;

            case GameState.GameOver:
                Cursor.visible = true;
            break;
        }
    }

    private void OnControlsChanged(PlayerInput input)
    {
        Debug.Log("Control scheme switched to: " + playerInput.currentControlScheme);
    }

    private void FindSteeringWheelDevice()
    {
        foreach (var device in InputSystem.devices)
        {
            if (device.name.Contains("Steering") || 
               (device.layout == "Joystick" && device.name.Contains("HID")))
            {
                steeringDevice = device;

                // axis baca child control
                var stickX = steeringDevice.TryGetChildControl<AxisControl>("stick/x");
                var stickY = steeringDevice.TryGetChildControl<AxisControl>("stick/y");

                if (stickX != null) centerX = stickX.ReadValue();
                if (stickY != null) centerY = stickY.ReadValue();

                Debug.Log($"Steering Wheel Found: {device.name}");
                Debug.Log($"Calibration centerX={centerX}, centerY={centerY}");

                // Disable Steering Action supaya tidak conflict
                if (playerInput.actions["Steering"] != null)
                    playerInput.actions["Steering"].Disable();

                break;
            }
        }
    }


    private void Move()
    {
        if(!isMove) return;

        // Cek apakah menggunakan steering wheel dengan pembacaan langsung
        if (useDirectInput && steeringDevice != null)
        {
            HandleSteeringWheelDirect();
            return;
        }
        
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard":
                Cursor.lockState = CursorLockMode.None; 
                Cursor.visible = true; 
                
                moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
                rb.velocity = moveInput * speed;
                break;

            case "Mouse":
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false; 

                pointerInput = playerInput.actions["Pointer"].ReadValue<Vector2>();
                Vector3 worldPos = mainCam.ScreenToWorldPoint(
                    new Vector3(pointerInput.x, pointerInput.y, Mathf.Abs(mainCam.transform.position.z))
                );
                Vector2 newPos = Vector2.Lerp(transform.position, worldPos, followSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                break;

            case "Gamepad":
                moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
                rb.velocity = moveInput * speed;
                break;

            case "SteeringWheel":
                HandleSteeringWheel();
                break;
        }
    }

    private void HandleSteeringWheelDirect()
    {
        var stickX = steeringDevice.TryGetChildControl<AxisControl>("stick/x");
        var stickY = steeringDevice.TryGetChildControl<AxisControl>("stick/y");

        float steerInput = 0f;
        float vertical = 0f;

        // Steering (with calibration)
        if (stickX != null)
        {
            float raw = stickX.ReadValue();

            // normalisasi agar idle = 0
            steerInput = Mathf.InverseLerp(centerX, -centerX, raw) * 2f - 1f;
            steerInput = -steerInput; // invert direction

            if (Mathf.Abs(steerInput) < deadzone)
                steerInput = 0f;
        }

        // Pedal gas/brake
        if (stickY != null)
        {
            float rawY = stickY.ReadValue();

            float normalized = Mathf.InverseLerp(centerY, -1f, rawY);

            if (normalized < deadzone)
                normalized = 0f;

            float gas = Mathf.Clamp01(normalized);
            float brake = Mathf.Clamp01(0.5f - normalized);

            vertical = (gas - brake) * verticalSpeed;
        }

        Vector2 vel = new Vector2(steerInput * steerSpeed, vertical);

        if (vel.magnitude > maxSpeed)
            vel = vel.normalized * maxSpeed;

        rb.velocity = vel;
    }

    private void HandleSteeringWheel()
    {
        float steerInput = playerInput.actions["Steering"].ReadValue<float>();
        float gasAxis = playerInput.actions["Gas"].ReadValue<float>();
        float brakeAxis = playerInput.actions["Brake"].ReadValue<float>();

        if (Mathf.Abs(steerInput) < deadzone) steerInput = 0f;
        if (Mathf.Abs(gasAxis) < deadzone) gasAxis = 0f;
        if (Mathf.Abs(brakeAxis) < deadzone) brakeAxis = 0f;

        float horizontal = (-steerInput) * steerSpeed;
        float vertical = (gasAxis - brakeAxis) * verticalSpeed;

        Vector2 targetVelocity = new Vector2(horizontal, vertical);
        
        if (targetVelocity.magnitude > maxSpeed)
            targetVelocity = targetVelocity.normalized * maxSpeed;

        rb.velocity = targetVelocity;
    }
}