using UnityEngine;

public class CameraOrbitScript : MonoBehaviour
{
    public Transform target;           // the menu focal point (logo)
    public float introDuration = 1.2f; // seconds
    public Vector3 introOffset = new Vector3(0f, 1.6f, -3.5f);
    public Vector3 orbitOffset = new Vector3(0f, 1.6f, -4.0f);
    public float orbitSpeed = 8f;      // degrees per second (slow)
    private float timer = 0f;
    private bool introDone = false;
    private Vector3 initialPos;

    void Start()
    {
        if (target == null) Debug.LogWarning("CameraIntroOrbit: Set target.");
        initialPos = target.position + introOffset;
        transform.position = initialPos;
        transform.LookAt(target.position + Vector3.up * 0.5f);
    }

    void Update()
    {
        if (target == null) return;
        timer += Time.deltaTime;
        if (!introDone)
        {
            float t = Mathf.Clamp01(timer / introDuration);
            // smooth step for nicer feel
            float s = Mathf.SmoothStep(0f, 1f, t);
            Vector3 desired = Vector3.Lerp(target.position + introOffset, target.position + orbitOffset, s);
            transform.position = desired;
            transform.LookAt(target.position + Vector3.up * 0.5f);

            if (t >= 1f) { introDone = true; timer = 0f; }
            return;
        }

        // idle orbit
        //transform.RotateAround(target.position + Vector3.up * 0.5f, Vector3.up, orbitSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 0.5f);
    }
}
