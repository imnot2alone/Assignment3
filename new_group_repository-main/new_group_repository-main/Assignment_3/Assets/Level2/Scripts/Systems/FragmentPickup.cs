using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FragmentPickup : MonoBehaviour
{
    public PartType type;
    
   

    private bool picked;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (picked) return;
        if (!other.CompareTag("Player")) return;

        picked = true;
        PartInventory.I?.Add(type);

        Destroy(gameObject);
    }
}