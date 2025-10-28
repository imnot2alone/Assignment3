using UnityEngine;

public class CollectiblePart : MonoBehaviour
{
    public PartType type;
    public AudioClip pickupSfx;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv == null) return;

        inv.Add(type);

        if (SFXplayer.Instance != null)
        {
            
            SFXplayer.Instance.PlayOneShot(pickupSfx, 1f, 1f);
        }

        Destroy(gameObject);
    }
}
