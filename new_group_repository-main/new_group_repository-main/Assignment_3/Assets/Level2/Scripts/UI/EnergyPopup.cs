using System.Collections;
using UnityEngine;

public class EnergyPopupr : MonoBehaviour
{
    [Header("Refs")]
    public PopupText popupPrefab;       
    public RectTransform popupLayer;   
    public Camera uiCamera;            

    [Header("Style")]
    public string format = "+{0} Energy";
    public Color color = new(0.2f, 1f, 0.5f);

    [Header("Merge")]
    public float mergeWindow = 0.25f;   
    public Vector2 headOffset = new(0f, 80f);

    int pending;                        
    float lastEventTime;                
    Vector3 lastWorldPos;               
    Coroutine flushCo;

    void OnEnable()  => EnergyPickup.OnPicked += Handle;
    void OnDisable() => EnergyPickup.OnPicked -= Handle;

    void Handle(int amount, Vector3 worldPos)
    {
        pending += amount;
        lastWorldPos = worldPos;
        lastEventTime = Time.unscaledTime;

        if (flushCo == null) flushCo = StartCoroutine(FlushWhenQuiet());
    }

    IEnumerator FlushWhenQuiet()
    {
        while (Time.unscaledTime - lastEventTime < mergeWindow)
            yield return null;

        SpawnPopup(pending, lastWorldPos);
        pending = 0;
        flushCo = null;
    }

    void SpawnPopup(int total, Vector3 worldAtHead)
    {
        if (!popupPrefab || !popupLayer) return;

        var inst = Instantiate(popupPrefab, popupLayer);
        var rt   = (RectTransform)inst.transform;

        // Canvas model detection：Overlay= null；Camera = uiCamera
        var canvas   = popupLayer.GetComponentInParent<Canvas>();
        var camForUI = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : uiCamera;

        Vector2 screen = Camera.main.WorldToScreenPoint(worldAtHead);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            popupLayer, screen, camForUI, out var local);

        rt.anchoredPosition = local + headOffset;
        inst.Play(string.Format(format, total), color);
    }
}
