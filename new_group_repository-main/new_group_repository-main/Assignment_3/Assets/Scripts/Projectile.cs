using UnityEngine;


public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damage = 10;


    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                HealthSystem.Instance.TakeDamage(10); // Take damage x points
            }
        }
        Destroy(gameObject);
    }
}
