using System.Collections.Generic;
using UnityEngine;

public class PartInventory : MonoBehaviour
{
    public static PartInventory I;

    [Header("PartRequired")]
    public int required = 3;

    
    private readonly HashSet<PartType> bag = new HashSet<PartType>();
    
    private readonly Dictionary<PartType, int> counts = new Dictionary<PartType, int>();

    void Awake() => I = this;

    
    public void Add(PartType t)
    {
        
        counts.TryGetValue(t, out int c);
        counts[t] = c + 1;

        
        bool firstTime = bag.Add(t);
        if (firstTime)
            HUD.I?.SetParts(bag.Count, required);
    }

    
    public bool TryUse(PartType type)
    {
        if (!counts.TryGetValue(type, out int c) || c <= 0)
            return false;

        c -= 1;
        if (c <= 0)
        {
            counts.Remove(type);
            
            if (bag.Remove(type))
                HUD.I?.SetParts(bag.Count, required);
        }
        else
        {
            counts[type] = c;
        }
        return true;
    }


    public bool Has(PartType t) => counts.TryGetValue(t, out int c) && c > 0;

   
    public int GetCount(PartType t) => counts.TryGetValue(t, out int c) ? c : 0;
}