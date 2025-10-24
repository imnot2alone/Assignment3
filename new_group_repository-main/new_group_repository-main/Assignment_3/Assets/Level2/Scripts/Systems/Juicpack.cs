using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JuicePack : MonoBehaviour
{
    [Header("Flash")]
    public Image flashImage;
    public float flashTime = 0.15f;

    [Header("Particles")]
    public ParticleSystem winBurst;  
    public ParticleSystem deathDust;  

    bool _winShown; 

    void Awake()
    {
        if (flashImage)
        {
            var c = flashImage.color;
            flashImage.color = new Color(c.r, c.g, c.b, 0f);
        }
    }

    void OnEnable()
    {
        Signals.TurbineBuilt += OnWin;
        Signals.PlayerKilled += OnDeath;
    }
    void OnDisable()
    {
        Signals.TurbineBuilt -= OnWin;
        Signals.PlayerKilled -= OnDeath;
    }

    void OnWin()
    {
        if (_winShown) return;   
        _winShown = true;

        PlayOnce(winBurst);
        StartCoroutine(Flash(Color.white, flashTime));
    }

    void OnDeath()
    {
        if (deathDust && GameManager.I && GameManager.I.player)
        deathDust.transform.position = GameManager.I.player.position;
        PlayOnce(deathDust);
        StartCoroutine(Flash(new Color(1f, 0.2f, 0.2f, 1f), flashTime)); 
    }

    static void PlayOnce(ParticleSystem ps)
    {
        if (!ps) return;
        var main = ps.main;
        main.loop = false;
        main.useUnscaledTime = true; 
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.Play(true);
    }

    IEnumerator Flash(Color color, float t)
    {
        if (!flashImage) yield break;

        float half = t * 0.5f;
        // fade in
        for (float x = 0; x < half; x += Time.unscaledDeltaTime)
        {
            float a = Mathf.Lerp(0f, color.a, x / half);
            flashImage.color = new Color(color.r, color.g, color.b, a);
            yield return null;
        }
        // fade out
        for (float x = 0; x < half; x += Time.unscaledDeltaTime)
        {
            float a = Mathf.Lerp(color.a, 0f, x / half);
            flashImage.color = new Color(color.r, color.g, color.b, a);
            yield return null;
        }
        flashImage.color = new Color(color.r, color.g, color.b, 0f);
    }
}
