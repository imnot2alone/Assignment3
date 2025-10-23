using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip partPickupClip;

    void Reset()
    {
        src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 0f; // 2D
        src.volume = 0.8f;
    }

    void OnEnable()
    {
        PartInventory.OnPartAdded += OnPart;
    }

    void OnDisable()
    {
        PartInventory.OnPartAdded -= OnPart;
    }

    void OnPart(PartType type, Vector3 pos)
    {
        if (partPickupClip && src) src.PlayOneShot(partPickupClip);
    }
}
