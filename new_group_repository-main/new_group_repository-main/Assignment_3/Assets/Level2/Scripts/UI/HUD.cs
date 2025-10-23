using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD I;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI partsText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI co2Text;

    [Header("Labels")]
    [SerializeField] private string partsLabel = "Parts";
    [SerializeField] private string energyLabel = "Energy";
    [SerializeField] private string co2Label = "CO2";

    [Header("Bars")]
    [SerializeField] public Image energyFill; // HUD/EnBar/Fill
    [SerializeField] public Image co2Fill;    // HUD/CoBar/Fill

    void Awake() => I = this;

    public void SetParts(int cur, int req)
    {
        if (partsText) partsText.text = $"{partsLabel} {cur}/{req}";
    }

    public void SetEnergy(int cur, int target)
    {
        if (energyText) energyText.text = $"{energyLabel} {cur}/{target}";
        if (energyFill)
        {
            float t = target > 0 ? (float)cur / target : 0f;
            energyFill.fillAmount = Mathf.Clamp01(t);
        }
    }

    public void SetCO2(float value) // value: 0..100
    {
        if (co2Text) co2Text.text = $"{co2Label} {Mathf.RoundToInt(value)}";
        if (co2Fill)
        {
            float t = Mathf.InverseLerp(0f, 100f, value); // 0→0, 100→1
            co2Fill.fillAmount = Mathf.Clamp01(t);
        }
    }
}
