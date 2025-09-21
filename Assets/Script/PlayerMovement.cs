using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.enabled = false;
    }

    private void OnEnable() 
    {
        GameManager.OnStateChanged += StateHandler;
    }

    void OnDisable()
    {
        GameManager.OnStateChanged -= StateHandler;
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void StateHandler(GameState newState)
    {
        if(newState == GameState.Ingame){
            sprite.enabled = true;
        }
    }

    private void ProcessInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }

}
