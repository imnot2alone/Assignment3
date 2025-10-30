using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SFXplayer3 : MonoBehaviour
{
    public static SFXplayer3 Instance;
    public AudioMixerGroup outputGroup;   

    AudioSource src;

    void Awake() {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        src = GetComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = false;
        src.spatialBlend = 0f;           
        if (outputGroup) src.outputAudioMixerGroup = outputGroup;
        DontDestroyOnLoad(gameObject);   
    }

    public void PlayOneShot(AudioClip clip, float vol = 1f, float pitch = 1f) {
        if (!clip) return;
        src.pitch = pitch;
        src.PlayOneShot(clip, vol);
    }
}