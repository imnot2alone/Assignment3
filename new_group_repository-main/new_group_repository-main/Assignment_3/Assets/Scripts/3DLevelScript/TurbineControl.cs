using UnityEngine;

public class TurbineController : MonoBehaviour
{
    [Header("旋转控制")]
    public MonoBehaviour spinScriptHost;  
    private Behaviour spinScriptCached;   


    public AudioSource windAudioSource;   
    public AudioClip windLoopClip;        

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
    }
}
