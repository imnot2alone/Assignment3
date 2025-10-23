using TMPro;
using UnityEngine;

public class EnergyPopup : MonoBehaviour
{
    [Header("Refs")]
    public PopupText popupPrefab;   // PopupText prefabs
    public Canvas overlayCanvas;    // Screen Space - Overlay Canvas

    [Header("Style")]
    public Color color = new(0.2f, 0.8f, 1f, 1f);
    public Vector2 screenOffset = new(0f, 80f); 

    void OnEnable()  => EnergyPickup.OnPicked += HandlePicked;
    void OnDisable() => EnergyPickup.OnPicked -= HandlePicked;

    void HandlePicked(Vector3 worldPos, int amount)
    {
        if (!popupPrefab) return;

        if (!overlayCanvas) overlayCanvas = FindOverlay();
        if (!overlayCanvas) return;

        // 
        var inst = Instantiate(popupPrefab, overlayCanvas.transform);
        var rt   = (RectTransform)inst.transform;

        Vector3 screen = Camera.main ? Camera.main.WorldToScreenPoint(worldPos) : worldPos;
        rt.position = screen + (Vector3)screenOffset;

        inst.Play($"+{amount} Energy", color);
    }

    Canvas FindOverlay()
    {
        var canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (var c in canvases)
            if (c.isActiveAndEnabled && c.renderMode != RenderMode.WorldSpace)
                return c;
        return null;
    }
}
