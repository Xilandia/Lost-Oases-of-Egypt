using System.Collections.Generic;
using _Scripts.Player.Management;
using _Scripts.Player.Unit;
using _Scripts.Utility.Entity;
using UnityEngine;

public class PlayerStarter : MonoBehaviour
{
    [SerializeField] private List<GameObject> initialUnits = new List<GameObject>();

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
                PlayerWorker pW = unit.GetComponent<PlayerWorker>();
                EntityHandler.instance.SetPlayerWorkerStats(pW, unit.name);
                PlayerManager.instance.workers.Add(pW);
            }
        }
    }
}
