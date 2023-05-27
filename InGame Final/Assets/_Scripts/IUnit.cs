using UnityEngine;

public class IUnit : Interactable
{
    public GameObject unitStatDisplay;
    
    public override void Awake()
    {
        base.Awake();
        unitStatDisplay.SetActive(false);
    }
    
    public override void OnInteractEnter()
    {
        base.OnInteractEnter();
        unitStatDisplay.SetActive(true);
    }

    public override void OnInteractExit()
    {
        base.OnInteractExit();
        unitStatDisplay.SetActive(false);
    }
}
