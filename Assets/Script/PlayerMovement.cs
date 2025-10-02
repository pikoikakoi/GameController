using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlDevice
{
    Keyboard, Mouse, Gamepad
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
    [SerializeField] private float followSpeed = 5f;    // kecepatan untuk mouse follow
    private Camera mainCam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        sprite.enabled = false;

        mainCam = Camera.main;
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

    private void Move()
    {
        if(!isMove) return;
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
        }
    }
}
