using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = Vector3.one * 1.06f;
    public float speed = 4f;
    private Vector3 baseScale;

    void Start() => baseScale = transform.localScale;

    void Update() => transform.localScale = Vector3.Lerp(transform.localScale, baseScale, Time.deltaTime * speed);

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        transform.localScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        transform.localScale = baseScale;
    }
}
