using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnergyPickup : MonoBehaviour
{
    public int value = 10;
    public string playerTag = "Player";

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (GameManager.I) GameManager.I.AddEnergy(value);
        Destroy(gameObject);
    }
}