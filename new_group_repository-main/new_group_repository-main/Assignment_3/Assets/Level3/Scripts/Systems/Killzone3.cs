using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Killzone3 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Signals.RaisePlayerKilled(); 
        GameManager3.I?.Respawn(other.transform);
        
    }
}