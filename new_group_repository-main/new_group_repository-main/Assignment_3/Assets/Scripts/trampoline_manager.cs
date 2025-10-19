using UnityEngine;

public class trampoline_manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float bounceforce = 14f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Newplayer"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                rb.AddForce(Vector2.up * bounceforce, ForceMode2D.Impulse);
            }
        }
    }
}
