using UnityEngine;

public class enemybird : MonoBehaviour
{

    [SerializeField] private float speed = 3f;     // Speed of movement
    [SerializeField] private float leftBound = -10f;  // Leftmost x-position
    [SerializeField] private float rightBound = 10f;  // Rightmost x-position
    [SerializeField] private bool movingRight = true; // Direction of travel

    private void Update()//update called once per frame
    {
        MoveBird();

    }
    private void MoveBird()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
                movingRight = false;
                //FlipSprite(); // turn left

        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
                movingRight = true;
            //FlipSprite();


        }
    }
    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip horizontally
        transform.localScale = scale;
    }
    // Update is called once per frame
    private void OnDrawGizmosSelected()
    {
        // Draw path bounds in editor for easier tuning
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftBound, transform.position.y, 0),
                        new Vector3(rightBound, transform.position.y, 0));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Newplayer")) // Make sure your player has this tag
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Apply a downward "bounce" force
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -8f); 
            }
        }
    }

    
}
