using TMPro;
using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class TurbineController : MonoBehaviour
{
    [Header("旋转控制")]
    public MonoBehaviour spinScriptHost;  
    private Behaviour spinScriptCached;   


    public AudioSource windAudioSource;   
    public AudioClip windLoopClip;

    [Header("Win System")]
    public float countdownTime = 30f;     // Countdown time in seconds
    public TMP_Text timerText;            // Assign a TextMeshPro text UI to show the timer
    public GameObject winScreen;          // Assign your Win Screen UI panel

    private bool isActive = false;

    void Start()
    {
       
        if (spinScriptHost != null)
        {
           
            spinScriptCached = spinScriptHost as Behaviour;
            if (spinScriptCached == null)
            {
          

                var hostGO = (spinScriptHost as Component)?.gameObject;
                if (hostGO != null)
                {
                    spinScriptCached = hostGO.GetComponent("TurbineSpin") as Behaviour;
                }
            }

            if (spinScriptCached != null)
            {
                spinScriptCached.enabled = false;
            }
        }

      
        if (windAudioSource != null)
        {
            windAudioSource.loop = false;
            windAudioSource.playOnAwake = false;
            windAudioSource.Stop();
        }

        // Hide win screen initially
        if (winScreen != null)
            winScreen.SetActive(false);
    }

    public void Activate()
    {
        if (isActive) return;
        isActive = true;

        if (spinScriptCached != null)
        {
            spinScriptCached.enabled = true;
        }
        else if (spinScriptHost != null)
        {
            // double check the turbinespin script
            var hostGO = (spinScriptHost as Component)?.gameObject;
            if (hostGO != null)
            {
                var spin = hostGO.GetComponent("TurbineSpin") as Behaviour;
                if (spin != null)
                {
                    spin.enabled = true;
                    spinScriptCached = spin;
                }
            }
        }

        // windclip loop
        if (windAudioSource != null && windLoopClip != null)
        {
            windAudioSource.clip = windLoopClip;
            windAudioSource.loop = true;

            // 3d voice
            windAudioSource.spatialBlend = 1f;   // 1 = 3D
            windAudioSource.dopplerLevel = 0f;
            windAudioSource.minDistance = 5f;
            windAudioSource.maxDistance = 50f;
            windAudioSource.volume = 0.5f;
            windAudioSource.Play();
        }

        // Start countdown coroutine
        StartCoroutine(CountdownTimer());
    }

    private IEnumerator CountdownTimer()
    {
        float timeLeft = countdownTime;

        while (timeLeft > 0f)
        {
            if (timerText != null)
                timerText.text = $"Turbine Energy Online in {timeLeft:F1}s";

            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        // Time’s up — show win screen
        if (winScreen != null)
            winScreen.SetActive(true);

        if (timerText != null)
            timerText.text = "Energy Online!";
    }
}
