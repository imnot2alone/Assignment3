using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Root")]
    public GameObject root;                 

    [Header("Controls")]
    public Slider volumeSlider;             
    public Toggle muteToggle;
    public TMP_Dropdown difficultyDropdown; 

    [Header("Audio (optional)")]
    public AudioMixer mixer;                
    public string volumeParam = "MasterVolume";

    [Header("Behaviour")]
    public KeyCode toggleKey = KeyCode.Escape;
    public bool startOpen = false;         

    const string KeyVol  = "opt.volume";
    const string KeyMute = "opt.mute";
    const string KeyDiff = "opt.diff";

    float _lastNonZero = 0.8f;
    bool  _open;

    void Awake()
    {
     
        if (difficultyDropdown && difficultyDropdown.options.Count == 0)
            difficultyDropdown.AddOptions(new System.Collections.Generic.List<string>{ "Easy","Normal","Hard" });

        LoadFromPrefs();
        if (startOpen) Show();
        else HideImmediate();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (_open) Hide();
            else Show();
        }
    }


    public void Show()
    {
        if (root) root.SetActive(true);
        _open = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    public void Hide()
    {
        if (root) root.SetActive(false);
        _open = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
    void HideImmediate()
    {
        if (root) root.SetActive(false);
        _open = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    // ===== UI  =====
    public void OnVolumeChanged(float v)
    {
        if (v > 0f) _lastNonZero = v;
        ApplyVolume(v);
        if (muteToggle && muteToggle.isOn && v > 0f) muteToggle.isOn = false;
        SaveToPrefs();
    }
    public void OnMuteToggled(bool on)
    {
        if (on)
        {
            ApplyVolume(0.0001f);
            if (volumeSlider) volumeSlider.interactable = false;
        }
        else
        {
            if (volumeSlider) volumeSlider.interactable = true;
            ApplyVolume(Mathf.Max(volumeSlider ? volumeSlider.value : _lastNonZero, 0.0001f));
        }
        SaveToPrefs();
    }
    public void OnDifficultyChanged(int idx)
    {
        DifficultyState.Set((Difficulty)idx);
        SaveToPrefs();
    }

    public void OnResumeClicked() => Hide();
    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("3DMenuScene"); // Build Settings
    }

    
    void ApplyVolume(float lin01)
    {
        lin01 = Mathf.Clamp(lin01, 0.0001f, 1f);
        if (mixer)
        {
            float dB = Mathf.Log10(lin01) * 20f;
            mixer.SetFloat(volumeParam, dB);
        }
        else
        {
            AudioListener.volume = lin01;
        }
        if (volumeSlider && !Mathf.Approximately(volumeSlider.value, lin01))
            volumeSlider.SetValueWithoutNotify(lin01);
    }
    void SaveToPrefs()
    {
        if (volumeSlider) PlayerPrefs.SetFloat(KeyVol, volumeSlider.value);
        PlayerPrefs.SetInt(KeyMute, (muteToggle && muteToggle.isOn) ? 1 : 0);
        PlayerPrefs.SetInt(KeyDiff, difficultyDropdown ? difficultyDropdown.value : 1);
        PlayerPrefs.Save();
    }
    void LoadFromPrefs()
    {
        float vol = PlayerPrefs.GetFloat(KeyVol, 0.8f);
        int mute  = PlayerPrefs.GetInt(KeyMute, 0);
        int diff  = PlayerPrefs.GetInt(KeyDiff, 1);

        if (volumeSlider)   volumeSlider.SetValueWithoutNotify(vol);
        if (muteToggle)     muteToggle.SetIsOnWithoutNotify(mute == 1);
        if (difficultyDropdown)
        {
            difficultyDropdown.value = Mathf.Clamp(diff, 0, 2);
            difficultyDropdown.RefreshShownValue();
            DifficultyState.Set((Difficulty)difficultyDropdown.value);
        }
        ApplyVolume(mute == 1 ? 0.0001f : vol);
    }

    void OnDestroy()
    {
    
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}


public enum Difficulty { Easy = 0, Normal = 1, Hard = 2 }
public static class DifficultyState
{
    public static Difficulty Current { get; private set; } = Difficulty.Normal;
    public static System.Action<Difficulty> OnChanged;
    public static void Set(Difficulty d)
    {
        if (Current == d) return;
        Current = d;
        OnChanged?.Invoke(d);
    }
}
