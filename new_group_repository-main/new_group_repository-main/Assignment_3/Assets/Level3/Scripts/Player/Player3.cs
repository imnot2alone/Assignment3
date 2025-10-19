using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using System;


public class Player3 : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float rotationSpeed = 20f;

    public float jumpForce = 7f;

    private bool isGrounded = false;

    private bool isleft = false;

    public EnergyManager em;

    //[Header("GroundCheck")]
    //public Transform groundCheckPos;
    //public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    //public LayerMask groundLayer;

    void Start()
    {
        Debug.Log("Human Controller Bad Example");
        PlayerPrefs.DeleteAll();
        

    }

    
    void Update()
    {
        Debug.Log("Frame " + Time.frameCount);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        

        //If the right key is pressed or d key is pressed add 1 to movement, OR : 0
        float rightKey = (Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)) ? 1f : 0f;
        //Same thing if pressing left
        float leftKey = (Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)) ? -1f : 0f;
        float axisInput = Input.GetAxis("Horizontal");

        float move = axisInput;
        //(axisInput + rightKey + leftKey) / 1.5f;

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
        //redundant below
        //transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime + Mathf.Sin(Time.time) * 0f);

        Vector2 place = transform.localScale;
        

        if (move > 0f && isleft)
        {
            isleft = !isleft;
            place.x *= 1;
            transform.localScale = place;

        }

        if (move < 0f && !isleft)
        {
            isleft = !isleft;
            place.x /= 1;
            transform.localScale = place;
        }

        //if (move < 0f) transform.localScale = new Vector2(-1f, 1f);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isGrounded = false;
            }



    }

    //private bool isGrounded()
    //{
    //    if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    //}

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
            em.energyCount++;   
        }
    }
}
   