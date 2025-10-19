using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MovingPlatform2D : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform pointA;         
    public Transform pointB;         
    [Header("Motion")]
    public float speed = 2f;         
    public float waitAtEnds = 0.3f;  

    Rigidbody2D rb;
    Vector2 A, B, target;
    float wait;
    Vector2 lastPos;
    public Vector2 Velocity { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;            
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Start()
    {
        
        A = pointA ? (Vector2)pointA.position : rb.position;
        B = pointB ? (Vector2)pointB.position : rb.position + Vector2.right * 3f;
        target = B;
        lastPos = rb.position;
    }

    void FixedUpdate()
    {
        if (wait > 0f)
        {
            wait -= Time.fixedDeltaTime;
            rb.linearVelocity = Vector2.zero;
            UpdateVel();
            return;
        }

        
        var pos = rb.position;
        var newPos = Vector2.MoveTowards(pos, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        UpdateVel();

       
        if ((newPos - target).sqrMagnitude < 0.0001f)
        {
            target = (target == B) ? A : B;
            wait = waitAtEnds;
        }
    }

    void UpdateVel()
    {
        Velocity = (rb.position - lastPos) / Time.fixedDeltaTime;
        lastPos = rb.position;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
       
        Gizmos.color = Color.cyan;
        var a = pointA ? pointA.position : transform.position;
        var b = pointB ? pointB.position : transform.position + Vector3.right * 3f;
        Gizmos.DrawSphere(a, 0.07f);
        Gizmos.DrawSphere(b, 0.07f);
        Gizmos.DrawLine(a, b);
    }
#endif
}