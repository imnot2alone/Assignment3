using System;
using System.Collections.Generic;
using UnityEngine;

public class PartInventory : MonoBehaviour
{
    public static PartInventory I;

    public static event Action<PartType, Vector3> OnPartAdded;

    [Header("PartRequired")]
    public int required = 3;

    readonly HashSet<PartType> bag = new();
    readonly Dictionary<PartType, int> counts = new();

    void Awake() => I = this;

    public void Add(PartType t, Vector3 worldPos)
    {
        counts.TryGetValue(t, out int c);
        counts[t] = c + 1;

        bool firstTime = bag.Add(t);
        if (firstTime) HUD.I?.SetParts(bag.Count, required);

        OnPartAdded?.Invoke(t, worldPos);
    }

    // 兼容旧调用
    public void Add(PartType t) => Add(t, Vector3.zero);

    public bool TryUse(PartType type)
    {
        if (!counts.TryGetValue(type, out int c) || c <= 0) return false;

        c -= 1;
        if (c <= 0)
        {
            counts.Remove(type);
            if (bag.Remove(type)) HUD.I?.SetParts(bag.Count, required);
        }
        else counts[type] = c;

        return true;
    }

    public bool Has(PartType t) => counts.TryGetValue(t, out int c) && c > 0;
    public int  GetCount(PartType t) => counts.TryGetValue(t, out int c) ? c : 0;
}
