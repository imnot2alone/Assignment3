using UnityEngine;

public class TurbineSpin : MonoBehaviour
{
    public float rpm = 20f;

    public Vector3 spinAxis = Vector3.right;

    void Update()
    {
        
        float degPerSec = rpm * 360f / 60f;
        transform.Rotate(spinAxis, degPerSec * Time.deltaTime, Space.Self);
    }
}
