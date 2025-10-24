using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelIntro : MonoBehaviour
{
    [Header("Refs")]
    public CanvasGroup canvasGroup;  
    public Image shade;             
    public TMP_Text lineText;        
    public TMP_Text hintText;       

    [Header("Content")]
    [TextArea(2,6)] public string[] lines;

    [Header("Typing")]
    public float charsPerSecond = 28f;
    public float puncPause = 0.25f;      // symbol pause time
    public float lineFadeTime = 0.8f;    // line fade time
    public float shadeAlpha = 1f;        // alpha

    [Header("Hint")]
    public string hint = "Press any key";
    public float hintFadeIn = 0.5f;
    public float hintBreathAmp = 0.15f;  // breth amp
    public float hintBreathHz  = 1.3f;   // breath frq

    [Header("Exit")]
    public float shadeFadeOut = 0.25f;   // exit fade time


    enum Stage { Typing, Shown, Fading, Hint, Done }
    Stage stage;
    bool  wantSkip;
    Color _line0, _hint0;

    void Awake()
    {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup) { canvasGroup.alpha = 1f; canvasGroup.interactable = false; canvasGroup.blocksRaycasts = false; }

        if (shade)
        {
            var c = shade.color; c.a = shadeAlpha; shade.color = c;
            shade.raycastTarget = false; 
        }

        _line0 = lineText ? lineText.color : Color.white;
        _hint0 = hintText ? hintText.color : Color.white;

        if (lineText) { lineText.text = ""; SetAlpha(lineText, 1f); }
        if (hintText)  hintText.text = hint;

        SetAlpha(hintText, 0f); 
    }

    void OnEnable() => StartCoroutine(Run());

    void Update()
    {
        if (!Input.anyKeyDown) return;

        switch (stage)
        {
            case Stage.Typing:
            case Stage.Shown:
                wantSkip = true; 
                break;

            case Stage.Hint:
               
                StartCoroutine(FinishAndHide());
                stage = Stage.Done;
                break;
        }
    }

    IEnumerator Run()
    {
        // if nothing display directly show hint
        if (lines == null || lines.Length == 0)
        {
            stage = Stage.Hint;
            yield return FadeText(hintText, 0f, 1f, hintFadeIn);
            yield break;
        }

        // Playing
        for (int i = 0; i < lines.Length; i++)
        {
            string s = lines[i];
            if (lineText) { lineText.text = ""; SetAlpha(lineText, 1f); }

            // 1) type
            wantSkip = false;
            stage = Stage.Typing;
            float dt = 1f / Mathf.Max(1f, charsPerSecond);

            for (int k = 0; k < s.Length; k++)
            {
                if (wantSkip) { if (lineText) lineText.text = s; break; }

                if (lineText) lineText.text += s[k];

                char ch = s[k];
                if (ch == '.' || ch == '!' || ch == '?' || ch == '，' || ch == '。' || ch == '！' || ch == '？')
                    yield return new WaitForSecondsRealtime(puncPause);
                else
                    yield return new WaitForSecondsRealtime(dt);
            }

            // 2) complete shown wait for pass or skip
            stage = Stage.Shown;
            float hold = 0.12f;
            float t = 0f;
            wantSkip = false;
            while (t < hold && !wantSkip)
            {
                t += Time.unscaledDeltaTime; yield return null;
            }

            // 3) fade out 
            stage = Stage.Fading;
            float fade = wantSkip ? 0.05f : lineFadeTime;
            if (lineText) yield return FadeText(lineText, 1f, 0f, fade);
        }

        // enter hint after line finish
        if (lineText) SetAlpha(lineText, 0f);
        stage = Stage.Hint;

        yield return FadeText(hintText, 0f, 1f, hintFadeIn);

        // breath
        while (stage == Stage.Hint)
        {
            float a = 1f - hintBreathAmp * 0.5f + Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f * hintBreathHz) * (hintBreathAmp * 0.5f);
            SetAlpha(hintText, a);
            yield return null;
        }
    }

    IEnumerator FinishAndHide()
    {
        // hide text
        if (hintText) yield return FadeText(hintText, hintText.color.a, 0f, 0.12f);

        // fade out 
        if (shade)
        {
            var c0 = shade.color; float a0 = c0.a;
            for (float t = 0; t < shadeFadeOut; t += Time.unscaledDeltaTime)
            {
                float a = Mathf.Lerp(a0, 0f, t / shadeFadeOut);
                var c = shade.color; c.a = a; shade.color = c;
                yield return null;
            }
            var c1 = shade.color; c1.a = 0f; shade.color = c1;
        }

        // close Intro panel（not effect other UI）
        gameObject.SetActive(false);
    }

    IEnumerator FadeText(TMP_Text txt, float from, float to, float dur)
    {
        if (!txt) yield break;
        for (float t = 0; t < dur; t += Time.unscaledDeltaTime)
        {
            float a = Mathf.Lerp(from, to, t / dur);
            SetAlpha(txt, a);
            yield return null;
        }
        SetAlpha(txt, to);
    }

    static void SetAlpha(TMP_Text t, float a)
    {
        var c = t.color; c.a = a; t.color = c;
    }
}
