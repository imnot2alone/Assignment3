using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class FallingPlatform : MonoBehaviour
{
    public float fallWait = 0.35f;     
    public float dropLife = 1.0f;      
    public float respawnDelay = 2.5f;  

    public string playerTag = "Player"; 

    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer[] rends;

    Vector3 startPos;
    Quaternion startRot;
    RigidbodyType2D startBodyType;

    bool busy;

    void Awake()
    {
        rb   = GetComponent<Rigidbody2D>();
        col  = GetComponent<Collider2D>();
        rends = GetComponentsInChildren<SpriteRenderer>(true);

        startPos      = transform.position;
        startRot      = transform.rotation;
        startBodyType = rb.bodyType;      
        rb.freezeRotation = true;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (busy) return;
        if (!c.collider.CompareTag(playerTag)) return;

        if (c.transform.position.y >= transform.position.y + 0.05f)
            StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        busy = true;

        yield return new WaitForSeconds(fallWait);

        // falling
        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(dropLife);

        
        col.enabled = false;
        foreach (var r in rends) r.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // respawn at same place
        rb.bodyType = startBodyType;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.SetPositionAndRotation(startPos, startRot);

        col.enabled = true;
        foreach (var r in rends) r.enabled = true;

        busy = false;
    }
}