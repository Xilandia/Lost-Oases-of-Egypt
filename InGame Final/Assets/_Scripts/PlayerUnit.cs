using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerUnit : MonoBehaviour, Damagable
{
    private NavMeshAgent navAgent;
    private Camera cam;

    public string unitName;
    public float unitCost, unitHealth, unitArmor, unitAttack, unitTimeBetweenAttacks, unitAttackRange, unitMoveSpeed, unitTrainTime;
    public float unitCurrentHealth;
    
    public GameObject unitStatDisplay;
    public Image unitHealthBarImage;
    
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    void Update()
    {
        HandleHealth();
    }
    
    public void MoveUnit(Vector3 _destination)
    {
        navAgent.SetDestination(_destination);
    }

    public void TakeDamage(float damage)
    {
        float totalDamage = damage - unitArmor;
        unitCurrentHealth -= totalDamage;
    }
    
    private void HandleHealth()
    {
        unitStatDisplay.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
        
        unitHealthBarImage.fillAmount = unitCurrentHealth / unitHealth;
        
        if (unitCurrentHealth <= 0)
        {
            UnitDeath();
        }
    }

    private void UnitDeath()
    {
        InputHandler.instance.selectedUnits.Remove(gameObject.transform);
        Destroy(gameObject);
    } 
}
