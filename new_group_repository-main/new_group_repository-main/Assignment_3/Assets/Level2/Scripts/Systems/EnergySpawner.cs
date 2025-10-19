using System.Collections;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject energyPrefab;

    [Header("Spawn Points")]
    public Transform[] points;          
    public float spawnRadius = 0.25f;   

    [Header("Spawn Rules")]
    public int  maxAlive     = 6;       
    public int  spawnPerTick = 1;      
    public float interval    = 1.0f;    

    [Header("Anti-overlap")]
    public float     minSpacing   = 0.4f;   
    public LayerMask pickupMask;            
    public LayerMask blockMask;             
    public int       maxTries     = 10;     

    int  alive;
    bool started;

    public void Begin()
    {
        if (started) return;
        started = true;
        StartCoroutine(Loop());
    }

    public void Stop()
    {
    started = false;
    }

public void ClearAllSpawned()
    {
    
    foreach (var go in GameObject.FindGameObjectsWithTag("Finish"))
        Destroy(go);
    alive = 0;
    }

    IEnumerator Loop()
    {
        var wait = new WaitForSeconds(interval);
        while (started)
        {
            
            int need = Mathf.Min(spawnPerTick, maxAlive - alive);
            for (int i = 0; i < need; i++)
                TrySpawnOne();

            yield return wait;
        }
    }

    void TrySpawnOne()
    {
        if (energyPrefab == null || points == null || points.Length == 0) return;

       
        for (int t = 0; t < maxTries; t++)
        {
            var p = points[Random.Range(0, points.Length)];
            if (!p) continue;

            Vector2 candidate = (Vector2)p.position + Random.insideUnitCircle * spawnRadius;

           
            if (Physics2D.OverlapCircle(candidate, minSpacing, pickupMask)) continue;

            
            if (blockMask.value != 0 && Physics2D.OverlapPoint(candidate, blockMask)) continue;

            var go = Instantiate(energyPrefab, candidate, Quaternion.identity);
            alive++;
            go.AddComponent<OnDestroyNotify>().onDestroyed = () => { alive = Mathf.Max(0, alive - 1); };
            return; 
        }
        
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (points == null) return;
        Gizmos.color = new Color(0f, 0.8f, 1f, 0.35f);
        foreach (var p in points)
            if (p) Gizmos.DrawWireSphere(p.position, spawnRadius);
    }
#endif
}

public class OnDestroyNotify : MonoBehaviour
{
    public System.Action onDestroyed;
    void OnDestroy() { onDestroyed?.Invoke(); }
}