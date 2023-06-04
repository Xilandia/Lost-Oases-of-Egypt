using System.Collections.Generic;
using _Scripts.Player.Management;
using _Scripts.Player.Unit;
using _Scripts.Utility.Entity;
using UnityEngine;

public class PlayerStarter : MonoBehaviour
{
    [SerializeField] private List<GameObject> initialUnits = new List<GameObject>();
    [SerializeField] private Entity worker;

    void Start()
    {
        foreach (GameObject prefab in initialUnits)
        {
            GameObject unit = Instantiate(prefab, transform.position, Quaternion.identity);
            unit.transform.SetParent(PlayerManager.instance.playerUnits);
            if (unit.CompareTag("Unit"))
            {
                PlayerUnit pU = unit.GetComponent<PlayerUnit>();
                EntityHandler.instance.SetPlayerUnitStats(pU, unit.name);

                if (pU.unitAttackRange > 5)
                {
                    PlayerManager.instance.rangedSoldiers.Add(pU);
                }
                else
                {
                    PlayerManager.instance.meleeSoldiers.Add(pU);
                }
            }
            else if (unit.CompareTag("Worker"))
            {
                // move to entity handler once properly implemented
                PlayerWorker pW = unit.GetComponent<PlayerWorker>();
                
                pW.workerName = worker.entityName;
                pW.workerHealth = worker.entityHealth;
                pW.workerCurrentHealth = worker.entityHealth;
                pW.workerArmor = worker.entityArmor;
                pW.workerOperationRange = worker.entityAttackRange;
                pW.workerMoveSpeed = worker.entityMoveSpeed;
                pW.workerGatherSpeed = worker.entityTimeBetweenAttacks;
                
                PlayerManager.instance.workers.Add(pW);
            }
        }
    }
}
