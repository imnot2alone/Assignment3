using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
   
    public float moveSpeed = 5f;     

  public float rotationSpeed = 20f;
    public float jumpForce = 7f;     

    public LayerMask groundLayer;    
    
    public EnergyManager em;        

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
     
      private bool isGrounded = false;
    void Awake()
    {
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr   = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
      
        float move = Input.GetAxisRaw("Horizontal"); // -1,0,1

    
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

   
        if (sr && Mathf.Abs(move) > 0.01f)
            sr.flipX = move < 0f;

   
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isGrounded = false;
            };


        if (anim)
        {
            bool moving = Mathf.Abs(rb.linearVelocity.x) > 0.05f;
            anim.SetBool("Moving", moving);
        }
    }

      void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 7)
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("pickup"))
        {
            col.gameObject.SetActive(false);
            if (em) em.energyCount++;
        }
    }
}
