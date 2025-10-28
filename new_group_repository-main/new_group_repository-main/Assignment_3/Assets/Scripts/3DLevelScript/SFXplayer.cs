using UnityEngine;
using UnityEngine.Audio; 

public class SFXplayer : MonoBehaviour
{
    public static SFXplayer Instance;

    public AudioSource source;

    public AudioClip[] clipsToPrewarm;

    void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

     
        if (source == null)
        {
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
        }

      //default avoid miss setting
        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0f; 
        source.dopplerLevel = 0f;
        source.volume = 1f;
        source.pitch = 1f;

       
        PrewarmClips();
    }

    private void PrewarmClips()
    {
        if (clipsToPrewarm == null) return;

        foreach (var clip in clipsToPrewarm)
        {
            if (clip == null) continue;

            if (clip.loadState == AudioDataLoadState.Unloaded)
            {
                clip.LoadAudioData(); 
            }
            if (clip.loadState == AudioDataLoadState.Loaded)
            {
                source.pitch = 1f;
                source.PlayOneShot(clip, 0f); 
            }
        }
    }

    public void PlayOneShot(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null || source == null) return;

        
        if (clip.loadState == AudioDataLoadState.Unloaded)
        {
            clip.LoadAudioData();
            return;
        }

        if (clip.loadState == AudioDataLoadState.Loading)
        {
            return;
        }


        if (clip.loadState == AudioDataLoadState.Failed)
        {
            return;
        }

      
        source.pitch = pitch;
        source.PlayOneShot(clip, volume);
    }
}
