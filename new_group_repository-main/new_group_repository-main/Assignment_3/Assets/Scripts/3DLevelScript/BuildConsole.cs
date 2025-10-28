using UnityEngine;

public class BuildConsole : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;

    public TurbineController turbineToActivate;

    public AudioSource audioSource;
    public AudioClip activateSfx;  

    public Renderer lampRenderer;   
    public Color colorBefore = Color.red;
    public Color colorAfter = Color.green;

    public Renderer screenRenderer;
    public Color screenBefore = new Color(0.2f, 0.2f, 0.2f);
    public Color screenAfter = new Color(0.5f, 0.9f, 0.5f);

    private PlayerInventory playerInv;
    private bool playerInRange = false;
    private bool alreadyActivated = false;

    void Start()
    {
     
        if (lampRenderer != null)
        {
            SetRendererColor(lampRenderer, colorBefore);
        }
        if (screenRenderer != null)
        {
            SetRendererColor(screenRenderer, screenBefore);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            playerInv = inv;
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var inv = other.GetComponent<PlayerInventory>();
        if (inv == playerInv)
        {
            playerInRange = false;
            playerInv = null;
        }
    }

    void Update()
    {
        if (!playerInRange || playerInv == null || alreadyActivated)
            return;

        if (Input.GetKeyDown(interactKey))
        {
            TryActivate();
        }
    }

    void TryActivate()
    {
        
        bool hasAll =
            playerInv.Has(PartType.Mast) &&
            playerInv.Has(PartType.Nacelle) &&
            playerInv.Has(PartType.Blade);

        if (!hasAll)
        {
            return;
        }


        playerInv.TryConsume(PartType.Mast);
        playerInv.TryConsume(PartType.Nacelle);
        playerInv.TryConsume(PartType.Blade);

      
        if (turbineToActivate != null)
        {
            turbineToActivate.Activate();
        }

    
     if (SFXplayer.Instance != null)
        {
            SFXplayer.Instance.PlayOneShot(activateSfx, 1f, 1.1f);
        }


       
        if (lampRenderer != null)
        {
            SetRendererColor(lampRenderer, colorAfter);
        }

     
        if (screenRenderer != null)
        {
            SetRendererColor(screenRenderer, screenAfter);
        }

        alreadyActivated = true;
    }

    void SetRendererColor(Renderer r, Color c)
    {
      
        if (r != null)
        {

            r.material.color = c;
        }
    }
}
