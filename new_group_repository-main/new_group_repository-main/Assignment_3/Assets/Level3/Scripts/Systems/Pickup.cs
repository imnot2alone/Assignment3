using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Pickup : MonoBehaviour
{
    public static event Action<Vector3, int> Picked;

    [Header("Config")]
    [SerializeField] string playerTag = "Player";
    [SerializeField] string takeTrigger = "Take";

    [Header("Audio")]               
    public AudioClip pickupSfx;        

    Animator _anim;
    Collider2D _col;
    bool _picked;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _col  = GetComponent<Collider2D>();
        if (_col) _col.isTrigger = true;
    }

    // key feature
    void OnEnable()
    {
        _picked = false;
        if (_col) _col.enabled = true;

        if (_anim)
        {
            _anim.Rebind();            // reset
            _anim.Update(0f);          // back to default
            _anim.ResetTrigger(takeTrigger);
            _anim.Play("Idle", 0, 0f); // start at idle
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_picked || !other.CompareTag(playerTag)) return;
        _picked = true;

        if (_col) _col.enabled = false;


        if (pickupSfx != null && SFXplayer3.Instance != null)         
        {
            SFXplayer3.Instance.PlayOneShot(pickupSfx, 1f, 1f);          
        }

        
        if (_anim) _anim.SetTrigger(takeTrigger);
        else DestroySelf();
    }

    // for Animation Event to use
    public void DestroySelf() => Destroy(gameObject);
}
