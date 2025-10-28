using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<PartType> owned = new HashSet<PartType>();
    public UnityEvent<PartType> OnPartCollected;
    public UnityEvent OnAllPartsReady;

    public bool Has(PartType t) => owned.Contains(t);
    public int Count => owned.Count;

    public void Add(PartType t)
    {
        if (owned.Add(t))
        {
            OnPartCollected?.Invoke(t);
            if (owned.Count >= 3) OnAllPartsReady?.Invoke();
        }
    }

    public bool TryConsume(PartType t)
    {
        if (!owned.Contains(t)) return false;
        owned.Remove(t);
        return true;
    }
}
