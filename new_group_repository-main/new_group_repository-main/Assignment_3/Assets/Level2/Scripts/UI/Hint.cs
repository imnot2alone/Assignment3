// Scripts/UI/FloatingHint.cs
using UnityEngine;

public class Hint : MonoBehaviour
{
    public CanvasGroup cg;
    public float baseAlpha = 0.85f;
    public float pulse = 0.15f;
    public float speed = 2f;

    void Reset() => cg = GetComponent<CanvasGroup>();
    void Update()
    {
        if (cg) cg.alpha = baseAlpha + Mathf.Sin(Time.time * speed) * pulse;
    }
}