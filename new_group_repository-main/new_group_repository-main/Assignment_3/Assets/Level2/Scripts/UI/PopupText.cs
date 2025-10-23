using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(TMP_Text))]
public class PopupText : MonoBehaviour
{
    [Header("Refs ")]
    [SerializeField] TMP_Text label;
    [SerializeField] CanvasGroup cg;

    [Header("Motion")]
    [Tooltip("life time)")] public float life = 0.8f;
    [Tooltip("location)")] public Vector2 move = new(0, 60);
    public AnimationCurve alpha = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public AnimationCurve scale = AnimationCurve.EaseInOut(0, 1, 1, 1.15f);

    void Awake()
    {
        if (!label) label = GetComponent<TMP_Text>();
        if (!cg)     cg    = GetComponent<CanvasGroup>();
    }

    
    public void Play(string msg, Color color)
    {
        if (!label) label = GetComponent<TMP_Text>();
        if (!cg)     cg    = GetComponent<CanvasGroup>();

        label.text  = msg;
        label.color = color;
        StopAllCoroutines();
        StartCoroutine(Anim());
    }
    public void Play(string msg) => Play(msg, label ? label.color : Color.white);

   
    public void Init(string msg, Vector2 localPos) =>
        PlayAtLocal(msg, label ? label.color : Color.white, localPos);

    public void Init(string msg, Vector2 localPos, Color color) =>
        PlayAtLocal(msg, color, localPos);

    
    public void PlayAtLocal(string msg, Color color, Vector2 localPos)
    {
        ((RectTransform)transform).anchoredPosition = localPos;
        Play(msg, color);
    }

    IEnumerator Anim()
    {
        var rt    = (RectTransform)transform;
        var start = rt.localPosition;
        var end   = start + (Vector3)move;

        for (float t = 0; t < life; t += Time.deltaTime)
        {
            float u = t / life;
            rt.localPosition = Vector3.Lerp(start, end, u);
            cg.alpha         = alpha.Evaluate(u);
            float s          = scale.Evaluate(u);
            rt.localScale    = Vector3.one * s;
            yield return null;
        }
        Destroy(gameObject);
    }
}
