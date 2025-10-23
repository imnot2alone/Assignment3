using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnergyPickup : MonoBehaviour
{
    // event pick energy(value and location)
    public static event Action<int, Vector3> OnPicked;


    public int amount = 10;      // energy amount

    bool consumed;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (consumed || !other.CompareTag("Player")) return;
        consumed = true;

        // update energy and co
        GameManager.I?.AddEnergy(amount);

        
        OnPicked?.Invoke(amount, other.transform.position + Vector3.up * 1.1f);

        Destroy(gameObject);
    }
}
