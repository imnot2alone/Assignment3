using UnityEngine;
using System.Linq;

public class WaypointPointer : MonoBehaviour
{

    public Transform player;               
    public RectTransform arrow;           
    public Camera cam;                      


    public string targetTag = "parts";      
    public float edgePadding = 40f;         
    public float hideDistance = 0.8f;        
    public bool hideWhenOnScreen = true;     
    public float rescanInterval = 0.25f;   

    Transform _target;

    void OnEnable()
    {
        if (cam == null) cam = Camera.main;
        InvokeRepeating(nameof(Rescan), 0f, Mathf.Max(0.05f, rescanInterval));

       
    }
    void OnDisable()
    {
        CancelInvoke(nameof(Rescan));

    }

    void OnAnyPickup(Vector3 _, int __) => Rescan();

    void Rescan()
    {
        var objs = GameObject.FindGameObjectsWithTag(targetTag);
        if (objs == null || objs.Length == 0)
        {
            SetArrowActive(false);
            _target = null;
            return;
        }

        
        var camUse = cam != null ? cam : Camera.main;
        Transform ChooseNearest(GameObject[] arr) =>
            arr.OrderBy(go => (go.transform.position - player.position).sqrMagnitude)
               .Select(go => go.transform).FirstOrDefault();

        bool IsOnScreen(Transform t)
        {
            var v = camUse.WorldToViewportPoint(t.position);
            return v.z > 0 && v.x > 0 && v.x < 1 && v.y > 0 && v.y < 1;
        }

        var off = objs.Where(go => go.activeInHierarchy && !IsOnScreen(go.transform)).ToArray();
        var on  = objs.Where(go => go.activeInHierarchy &&  IsOnScreen(go.transform)).ToArray();

        _target = (off.Length > 0 ? ChooseNearest(off) : ChooseNearest(on));

        SetArrowActive(_target != null);
    }

    void LateUpdate()
    {
        if (_target == null || player == null || arrow == null)
        {
            SetArrowActive(false);
            return;
        }

        var camUse = cam != null ? cam : Camera.main;
        if (camUse == null) { SetArrowActive(false); return; }

       
        if (!_target.gameObject.activeInHierarchy) { SetArrowActive(false); return; }

        var view = camUse.WorldToViewportPoint(_target.position);
        bool onScreen = view.z > 0 && view.x > 0 && view.x < 1 && view.y > 0 && view.y < 1;

      
        float dist = Vector2.Distance(_target.position, player.position);
        if (!hideWhenOnScreen && dist <= hideDistance) { SetArrowActive(false); return; }

        if (hideWhenOnScreen && onScreen)
        {
            
            SetArrowActive(false);
            return;
        }

       
        Vector3 sp = camUse.WorldToScreenPoint(_target.position);
        if (sp.z < 0) sp *= -1f; 
        float w = Screen.width, h = Screen.height;
        Vector2 clamped = new Vector2(
            Mathf.Clamp(sp.x, edgePadding, w - edgePadding),
            Mathf.Clamp(sp.y, edgePadding, h - edgePadding)
        );

        arrow.position = clamped;

        Vector2 dir = ((Vector2)sp - clamped).normalized;
        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        arrow.rotation = Quaternion.Euler(0, 0, ang);

        SetArrowActive(true);
    }

    void SetArrowActive(bool on)
    {
        if (arrow != null && arrow.gameObject.activeSelf != on)
            arrow.gameObject.SetActive(on);
    }
}