using UnityEngine;
public class Spin : MonoBehaviour {
    public float rpm = 60f;
    void Update() => transform.Rotate(0, 0, rpm * 6f * Time.deltaTime);
}