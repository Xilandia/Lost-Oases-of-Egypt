using UnityEngine;

public class PlayerTrainer : MonoBehaviour
{

    public string trainerName;
    public float trainerCost, trainerHealth, trainerArmor, trainerBuildTime;
    public float trainerCurrentHealth;
    public bool isBuildable, isPrototype, isPlaced, isComplete;
    public Entity[] buildableUnits;

    private bool constructionStarted = false;
    private float currProgress, initYScale;

    public void UpdatePrototypePosition()
    {
        if (isPrototype)
        {
            transform.position = BuildingSystem.SnapToGrid(BuildingSystem.GetMouseWorldPosition());
        }
    }

    public void StartConstruction()
    {
        constructionStarted = true;
        isPlaced = true;
        currProgress = 0f;
        Vector3 scale = transform.localScale;
        initYScale = scale.y;
        transform.localScale = new Vector3(scale.x, initYScale / 100, scale.z);
    }

    void Update()
    {
        if (constructionStarted)
        {
            currProgress += Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, initYScale * (currProgress / trainerBuildTime), transform.localScale.z);
            if (currProgress >= trainerBuildTime)
            {
                CompleteConstruction();
            }
        }
    }
    
    public void CompleteConstruction()
    {
        isComplete = true;
        transform.localScale = new Vector3(transform.localScale.x, initYScale, transform.localScale.z);
    }
    
}
