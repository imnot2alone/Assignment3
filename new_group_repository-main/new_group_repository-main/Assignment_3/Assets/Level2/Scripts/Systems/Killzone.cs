using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Killzone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Signals.RaisePlayerKilled(); 
        GameManager.I?.Respawn(other.transform);
        
    }
}