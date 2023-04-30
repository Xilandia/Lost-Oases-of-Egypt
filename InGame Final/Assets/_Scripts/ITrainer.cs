using UnityEngine;

public class ITrainer : Interactable
{
    public Entity playerTrainer;
    
    public override void OnInteractEnter()
    {
        ActionFrame.instance.SetActionButtons(playerTrainer);
        base.OnInteractEnter();
    }

    public override void OnInteractExit()
    {
        ActionFrame.instance.ClearAction();
        base.OnInteractExit();
    }
}
