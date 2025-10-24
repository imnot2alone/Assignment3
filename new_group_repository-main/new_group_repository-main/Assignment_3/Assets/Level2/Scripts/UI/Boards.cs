using UnityEngine;
using TMPro;

public class Boards : MonoBehaviour
{
    [Header("Refs")]
    public TextMeshProUGUI text;      
    public PartInventory inventory;   
    public GameManager gm;            

    [Header("Format")]
    [TextArea] public string partsFormat  = "Collect Turbine parts {0}/{1}";
    [TextArea] public string energyFormat = "Collect Clean energy {0}/{1}";
    [TextArea] public string doneText     = "Completed!";

    float nextCheck;

    void Awake()
    {
        if (!gm) gm = GameManager.I;
        if (!inventory) inventory = FindAnyObjectByType<PartInventory>();
    }

    void Start() => Refresh(true);

    void Update()
    {
        if (Time.time < nextCheck) return;
        nextCheck = Time.time + 0.25f;  
        Refresh(false);
    }
        
         void OnEnable()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        cg.ignoreParentGroups = true; 
    }

    void Refresh(bool force)
    {
        if (!text) return;
        if (!gm) gm = GameManager.I;

        string msg;

       
        if (gm && gm.energy >= gm.targetEnergy)
        {
            msg = doneText;
        }
       
        else if (gm && gm.turbineBuilt)
        {
            msg = string.Format(energyFormat, gm.energy, gm.targetEnergy);
        }
      
        else
        {
            int have = 0, req = 3;

            if (inventory)
            {
         
                if (TryGetInventoryTotals(inventory, out int c, out int r))
                {
                    have = c; req = r;
                }
                else
                {
                    
                    have = inventory.GetCount(PartType.Mast)
                         + inventory.GetCount(PartType.Nacelle)
                         + inventory.GetCount(PartType.Blade);
                    req = inventory.required;
                }
            }

            have = Mathf.Clamp(have, 0, req);
            msg = string.Format(partsFormat, have, req);
        }

        if (force || text.text != msg) text.text = msg;
    }

  
    bool TryGetInventoryTotals(PartInventory inv, out int collected, out int required)
    {
        collected = 0; required = 0;
      
        try
        {
            collected = (int)inv.GetType().GetProperty("Collected")?.GetValue(inv);
            required  = (int)inv.GetType().GetProperty("Required") ?.GetValue(inv);
            return required > 0;
        }
        catch { return false; }
    }
}