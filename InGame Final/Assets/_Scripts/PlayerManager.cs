using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Transform playerUnits;
    public Transform enemyUnits;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        UnitHandler.instance.SetEntityStats(playerUnits);
        //UnitHandler.instance.SetEntityStats(enemyUnits);
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler.instance.HandleUnitMovement();
    }
}
