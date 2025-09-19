using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float speed;
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // rb.velocity = Vector2.left * speed;
        // rb.velocity = new Vector2(-speed, rb.velocity.y);
    }

    void FixedUpdate()
    {
        // hanya overwrite kecepatan X, biarkan Y dari Animator
        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }
}
