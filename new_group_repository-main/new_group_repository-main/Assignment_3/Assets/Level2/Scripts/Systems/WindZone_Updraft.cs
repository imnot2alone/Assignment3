using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WindZone2D : MonoBehaviour
{
    public Vector2 direction = Vector2.up;
    public float strength = 35f;             
    public LayerMask affectLayers;

    void Reset() { GetComponent<Collider2D>().isTrigger = true; }

void OnTriggerStay2D(Collider2D other){
    if (((1 << other.gameObject.layer) & affectLayers) == 0) return;
    var rb = other.attachedRigidbody;
    if (!rb) return;
    
    rb.AddForce(direction.normalized * strength * rb.mass, ForceMode2D.Force);
}
}