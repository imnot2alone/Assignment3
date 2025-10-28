using UnityEngine;

public class Earthspin : MonoBehaviour
{

    // Update is called once per frame
    public float spinspeed = 50f;
    void Update()
    {
        transform.Rotate(Vector3.forward * spinspeed * Time.deltaTime);
    }
}
