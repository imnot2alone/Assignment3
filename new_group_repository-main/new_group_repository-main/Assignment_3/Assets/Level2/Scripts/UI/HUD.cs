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
    public Image energyFill; 
    public Image co2Fill; 

    void Awake() => I = this;

    // 碎片：显示 “Parts x/y”
    public void SetParts(int cur, int req)
    {
        if (partsText) partsText.text = $"{partsLabel} {cur}/{req}";
    }

    // 能量：显示 “Energy cur/target”，并可选更新进度条
    public void SetEnergy(int cur, int target)
    {
        if (energyText) energyText.text = $"{energyLabel} {cur}/{target}";
        if (energyFill && target > 0) energyFill.fillAmount = Mathf.Clamp01(cur / (float)target);
    }

    // CO2：显示 “CO2 n”
    public void SetCO2(float value)
    {
        if (co2Text) co2Text.text = $"{co2Label} {Mathf.RoundToInt(value)}";
    }
}