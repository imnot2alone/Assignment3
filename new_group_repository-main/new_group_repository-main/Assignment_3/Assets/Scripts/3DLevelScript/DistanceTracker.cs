using UnityEngine;
using TMPro;

public class ThreeTargetHUD : MonoBehaviour
{
    [Header("References")]
    public Transform player;           // Player transform
    public Transform target1;          // First target
    public Transform target2;          // Second target
    public Transform target3;          // Third target
    public TMP_Text distanceText;      // TextMeshPro UI element

    [Header("Settings")]
    public bool showDecimals = true;   // Show 2 decimal places if true

    void Update()
    {
        if (player == null || distanceText == null)
            return;

        // Build the distance info string
        string info = "";

        info += GetTargetDistanceInfo("Turbine Mast", target1);
        info += GetTargetDistanceInfo("Turbine Nacelle", target2);
        info += GetTargetDistanceInfo("Turbine Blade", target3);

        distanceText.text = info;
    }

    string GetTargetDistanceInfo(string label, Transform target)
    {
        if (target == null)
            return $"{label}: Collected\n";

        float distance = Vector3.Distance(player.position, target.position);
        string formattedDistance = showDecimals ? $"{distance:F2}" : $"{Mathf.RoundToInt(distance)}";
        return $"{label}: {formattedDistance} m\n";
    }
}
