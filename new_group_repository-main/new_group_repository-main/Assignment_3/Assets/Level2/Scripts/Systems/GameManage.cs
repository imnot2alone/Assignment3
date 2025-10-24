using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("References")]
    public Transform player;
    public Transform defaultSpawn;

    [Header("Energy / CO2")]
    public int   energy       = 0;     
    public int   targetEnergy = 100;   
    public float co2          = 100f;  
    public float co2PerEnergy = 1f;    

    [Header("Spawner (optional)")]
    public EnergySpawner energySpawner;
    public bool startSpawnerOnTurbineBuilt = true;

    [Header("Win UI")]
    public WinPanel winPanel;          

    Vector3 _respawn;

    void Awake()
    {
        I = this;
        _respawn = defaultSpawn ? defaultSpawn.position : Vector3.zero;
    }

    void Start()
    {
        
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        if (player)
        {
            player.position = _respawn;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb) { rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; }
        }

        
        HUD.I?.SetEnergy(energy, targetEnergy);
        HUD.I?.SetCO2(co2);
    
    }

    public void SetCheckpoint(Vector3 pos) => _respawn = pos;

    public void Respawn(Transform who)
    {
        who.position = _respawn;
        var rb = who.GetComponent<Rigidbody2D>();
        if (rb) { rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; }
    }

   
    public void AddEnergy(int amount)
    {
        if (amount <= 0) return;

        energy = Mathf.Clamp(energy + amount, 0, targetEnergy);
        co2    = Mathf.Max(0f, co2 - amount * co2PerEnergy);   

        HUD.I?.SetEnergy(energy, targetEnergy);
        HUD.I?.SetCO2(co2);

        if (energy >= targetEnergy)
            OnEnergyGoalReached();
    }

    public void ResetEnergyAndCO2(int energyValue = 0, float co2Value = 100f)
    {
        energy = Mathf.Max(0, energyValue);
        co2    = Mathf.Clamp(co2Value, 0f, 100f);
        HUD.I?.SetEnergy(energy, targetEnergy);
        HUD.I?.SetCO2(co2);
    }
    public bool turbineBuilt;
    public void OnTurbineBuilt()
    {
        Signals.RaiseTurbineBuilt(); 
        turbineBuilt = true;
         if (startSpawnerOnTurbineBuilt && energySpawner) energySpawner.Begin();
    }

    void OnEnergyGoalReached()
    {
        Debug.Log("[GM] Energy goal reached!");
        PlayerPrefs.SetInt("Level2Completed", 1);
        PlayerPrefs.Save();

       
        if (energySpawner) energySpawner.Stop();
        if (winPanel) winPanel.Show(energy, targetEnergy, co2);
    }
}