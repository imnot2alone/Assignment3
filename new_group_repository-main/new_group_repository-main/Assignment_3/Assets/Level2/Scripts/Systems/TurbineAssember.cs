using UnityEngine;
using System.Collections;

public class TurbineAssembler : MonoBehaviour
{
    [Header("PartRender")]
    public SpriteRenderer mastRenderer;
    public SpriteRenderer nacelleRenderer;
    public SpriteRenderer bladeRenderer;

    [Header("IncompleteAlpha")]
    [Range(0f,1f)] public float incompleteAlpha = 0.35f;

    [Header("Windzones & Rotor")]
    public GameObject[] windZones;      
    public Spin rotorSpin;
    public float targetRPM = 120f;
    public float rampTime = 1.2f;

    [Header("Energy Spawner")]
    public EnergySpawner energySpawner;
    public bool startSpawnerOnBuilt = true;

    [Header("Audio")]                                     
    public AudioClip partInstallSfx;                       
    public AudioClip completeSfx;                          
    [Range(0f, 2f)] public float partInstallVolume = 1f;    
    [Range(0f, 2f)] public float completeVolume = 1f;     

    bool mastDone, nacelleDone, bladeDone;

    void Start()
    {
        SetAlpha(mastRenderer,    incompleteAlpha);
        SetAlpha(nacelleRenderer, incompleteAlpha);
        SetAlpha(bladeRenderer,   incompleteAlpha);

        SetWindZonesActive(false);            
        if (rotorSpin) { rotorSpin.enabled = false; rotorSpin.rpm = 0f; }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = PartInventory.I;
        if (!inv) return;

        // ----- Mast -----
        if (!mastDone && inv.TryUse(PartType.Mast))
        {
            mastDone = true;
            SetAlpha(mastRenderer, 1f);

           
            PlayPartInstallSfx();                         
        }

        // ----- Nacelle -----
        if (!nacelleDone && inv.TryUse(PartType.Nacelle))
        {
            nacelleDone = true;
            SetAlpha(nacelleRenderer, 1f);

           
            PlayPartInstallSfx();                             
        }

        // ----- Blade -----
        if (!bladeDone && inv.TryUse(PartType.Blade))
        {
            bladeDone = true;
            SetAlpha(bladeRenderer, 1f);

            
            PlayPartInstallSfx();                             
        }

        // 全部完成 → 只执行一次
        if (mastDone && nacelleDone && bladeDone)
        {
            Complete();
            enabled = false; 
        }
    }

    void Complete()
    {
        SetWindZonesActive(true);                 
        if (rotorSpin && !rotorSpin.enabled) StartCoroutine(RampUpSpin());
        if (startSpawnerOnBuilt && energySpawner) energySpawner.Begin();

       
        PlayCompleteSfx();                                   

        GameManager.I?.OnTurbineBuilt();
        Signals.RaiseTurbineBuilt();

        enabled = false;
    }

    void SetWindZonesActive(bool on)
    {
        if (windZones == null) return;
        foreach (var z in windZones)
            if (z) z.SetActive(on);
    }

    IEnumerator RampUpSpin()
    {
        rotorSpin.enabled = true;
        float t = 0f, start = rotorSpin.rpm;
        while (t < rampTime)
        {
            t += Time.deltaTime;
            rotorSpin.rpm = Mathf.Lerp(start, targetRPM, t / rampTime);
            yield return null;
        }
        rotorSpin.rpm = targetRPM;
    }

    static void SetAlpha(SpriteRenderer r, float a)
    {
        if (!r) return;
        var c = r.color; 
        c.a = a; 
        r.color = c;
    }



    void PlayPartInstallSfx()                                
    {
        if (partInstallSfx != null && SFXplayer.Instance != null)
        {
            SFXplayer.Instance.PlayOneShot(partInstallSfx, partInstallVolume, 1f);
        }
    }

    void PlayCompleteSfx()                                     
    {
        if (completeSfx != null && SFXplayer.Instance != null)
        {
            SFXplayer.Instance.PlayOneShot(completeSfx, completeVolume, 1f);
        }
    }
}
