using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FragmentPickup : MonoBehaviour
{
    public PartType type;

    [Header("Popup ")]
    public PopupText popupPrefab;        
    public RectTransform popupLayer;     
    public Camera uiCamera;              

    [Header("Text")]
    public string popupText = "+1 Fragment";
    public Color  popupColor = new(1f, 0.93f, 0.25f);

    [Header("Offsets")]
    public Vector2 headOffset = new(0f, 80f); 

    bool picked;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (picked || !other.CompareTag("Player")) return;
        picked = true;

        //  add part type to inventory
        PartInventory.I?.Add(type);

        // popuptext
        if (popupPrefab && popupLayer)
        {
            var inst = Instantiate(popupPrefab, popupLayer);
            var rt   = (RectTransform)inst.transform;

            // world to screen
            Vector3 world  = other.transform.position + Vector3.up * 1.1f;
            Vector2 screen = Camera.main.WorldToScreenPoint(world);

            // Canvas model detection：Overlay= null；Camera = uiCamera
            var canvas = popupLayer.GetComponentInParent<Canvas>();
            var camForUI = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : uiCamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                popupLayer, screen, camForUI, out var local);

            rt.anchoredPosition = local + headOffset;   // ★  anchoredPosition
            inst.Play(popupText, popupColor);
        }

        
        Destroy(gameObject);
    }
}
