using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    public float maxSpeed = 6f;
    public float accel = 30f;       
    public float jumpForce = 12f;
    public LayerMask groundLayer;

    Rigidbody2D rb;

    void Awake(){ rb = GetComponent<Rigidbody2D>(); }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.AddForce(Vector2.right * h * accel, ForceMode2D.Force);

       
        var v = rb.linearVelocity;
        v.x = Mathf.Clamp(v.x, -maxSpeed, maxSpeed);
        rb.linearVelocity = v;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
    }
}