using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform playerUnits;
    public Transform playerTrainers;
    public Transform enemyUnits;
    
    
    void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        EntityHandler.instance.SetEntityStats(playerUnits);
        EntityHandler.instance.SetEntityStats(playerTrainers);
        //UnitHandler.instance.SetEntityStats(enemyUnits);
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler.instance.HandlePlayerInput();
    }
}
